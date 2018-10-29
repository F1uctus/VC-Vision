using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using PluginInterface;

namespace Vision {
    public class Plugin : IPlugin {
        internal CtlMain MainCtl;

        public Plugin() {
            MainCtl       = new CtlMain();
            MainInterface = MainCtl;
        }

        #region Required plugin attributes

        // Declarations of all our internal plugin variables.
        // VoxCommando expects these to be here so don't remove them.

        // All properties binded to their definitions in
        // assembly information to simplify development.
        // Change properties in: (Visual Studio)
        // Project -> <Project> properties -> Application -> Assembly Information

        public string Name { get; } = nameof(Vision);

        public string Description { get; } =
            Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                    .OfType<AssemblyDescriptionAttribute>()
                    .FirstOrDefault()?.Description;

        public string Author { get; } =
            Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
                    .OfType<AssemblyCompanyAttribute>()
                    .FirstOrDefault()?.Company;

        public bool GenEvents { get; } = true;

        internal static IPluginHost HostInstance;

        public IPluginHost Host {
            get { return HostInstance; }
            set {
                HostInstance = value;
                MainCtl      = (CtlMain) MainInterface;
            }
        }

        public string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public UserControl MainInterface { get; }

        #endregion

        /// <summary>
        ///     First function called by the host.
        ///     Put anything needed to start with here first.
        ///     At this point the host object exists.
        ///     This function is called in a thread so be careful to handle all errors.
        ///     e.g. udpListener.myHost = this.Host;
        /// </summary>
        public void Initialize() {
            if (PluginOptions.CascadesPath.ToUpper().Any(letter => "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ".Contains(letter))) {
                MessageBox.Show(
                    "Path to cascade doesn't support Cyrillic symbols. Please, move program folder to path without Cyrillic symbols.",
                    "Vision plugin warning"
                );
            }
            // Loading photos of trained persons.
            for (var faceI = 0; faceI < PluginOptions.PeopleFaces.Count; faceI++) {
                MainCtl.TrainedImages.Add(new Image<Gray, byte>($"{PluginOptions.PluginPath}Faces\\face{faceI + 1}.bmp"));
            }
        }

        /// <summary>
        ///     This is the main method called by VoxCommando when performing plugin actions. All actions go through here.
        /// </summary>
        /// <param name="actionNameArray">
        ///     An array of strings representing action name. Example action: xbmc.send >>>
        ///     actionNameArray[0] is the plugin name (xbmc), actionNameArray[1] is "send".
        /// </param>
        /// <param name="actionParameters">An array of strings representing our action parameters.</param>
        /// <returns>an actionResult</returns>
        public actionResult doAction(string[] actionNameArray, string[] actionParameters) {
            var          ar            = new actionResult();
            const string unknownAction = "Unknown " + nameof(Vision) + " plugin action.";
            try {
                Task.Run(
                    delegate {
                        switch (actionNameArray[1].ToUpper()) {
                            case "PHOTO":
                                using (var capture = new VideoCapture()) {
                                    Thread.Sleep(PluginOptions.CameraDelayMs);
                                    using (Image<Bgr, byte> imageFrame = capture.QueryFrame().ToImage<Bgr, byte>()) {
                                        SaveImage(imageFrame.Bitmap);
                                    }
                                }
                                ar.setInfo("Capture saved to plugin directory.");
                                break;
                            case "DETECT":
                                ar = FindObjects(actionParameters);
                                break;
                            case "OCR":
                                ar = TeserractOcr(actionParameters);
                                break;
                            case "FACETRAIN":
                                ar = TrainFace(actionParameters);
                                break;
                            case "FACERECO":
                                ar = RecognizeFace(actionParameters);
                                break;
                            case "MOTIONSENSOR":
                                throw new NotImplementedException();
                            default:
                                ar.setError(unknownAction);
                                break;
                        }
                    }
                );
            }
            catch (Exception err) {
                ar.setError(err.ToString());
            }
            return ar;
        }

        /// <summary>
        ///     Releases all unmanaged resources (which implements IDisposable)
        /// </summary>
        public void Dispose() {
            // You must release all unmanaged resources here when program is stopped to prevent the memory leaks.
        }

        #region other methods

        /// <param name="parameters">
        ///     [0] - Path to image,
        ///     [1] - Cascade 1,
        ///     [2] - Cascade 2 ...
        /// </param>
        private actionResult FindObjects(string[] parameters) {
            var ar = new actionResult();

            #region Parameters parsing

            switch (parameters.Length) {
                case 0: {
                    ar.setError("Path to image not specified.");
                    return ar;
                }
                case 1: {
                    ar.setError("Cascade name not specified.");
                    return ar;
                }
            }

            Image<Gray, byte> grayImage;
            if (string.IsNullOrEmpty(parameters[0])) {
                using (var capture = new VideoCapture()) {
                    Thread.Sleep(PluginOptions.CameraDelayMs);
                    grayImage = capture.QueryFrame().ToImage<Gray, byte>();
                }
            }
            else {
                try {
                    grayImage = new Image<Gray, byte>(parameters[0]);
                }
                catch {
                    ar.setError("Invalid path to image.");
                    return ar;
                }
            }

            if (PluginOptions.UseImageCorrection) {
                grayImage._EqualizeHist();
            }

            #endregion

            var resultString = "";
            for (var i = 1; i < parameters.Length; i++) {
                if (string.IsNullOrEmpty(parameters[i])) {
                    continue;
                }

                using (var classifier = new CascadeClassifier($"{PluginOptions.CascadesPath}haarcascade_{parameters[i].ToLower().Trim()}.xml")) {
                    Rectangle[] objects = classifier.DetectMultiScale(grayImage, 1.1, 10);
                    if (objects.Length != 0) {
                        for (var index = 0; index < objects.Length; index++) {
                            grayImage.Draw(objects[index], new Gray(0), 2);
                        }
                    }
                    resultString += $"<{parameters[i]}:{objects.Length}>\n";
                }
            }
            SaveImage(grayImage.Bitmap);
            grayImage.Dispose();
            ar.setSuccess(resultString);
            return ar;
        }

        private actionResult TeserractOcr(string[] parameters) {
            var ar = new actionResult();

            #region Parameters parsing

            switch (parameters.Length) {
                case 0: {
                    ar.setError("Path to image not specified.");
                    return ar;
                }
                case 1: {
                    ar.setError("Symbols culture not specified.");
                    return ar;
                }
            }

            Image<Gray, byte> grayImage;
            if (string.IsNullOrEmpty(parameters[0])) {
                using (var capture = new VideoCapture()) {
                    Thread.Sleep(PluginOptions.CameraDelayMs);
                    grayImage = capture.QueryFrame().ToImage<Gray, byte>();
                }
            }
            else {
                try {
                    grayImage = new Image<Gray, byte>(parameters[0]);
                }
                catch {
                    ar.setError("Invalid path to image.");
                    return ar;
                }
            }

            if (PluginOptions.UseImageCorrection) {
                grayImage._EqualizeHist();
            }

            #endregion

            using (Tesseract tesseract = parameters.Length == 2
                                             ? new Tesseract($"{PluginOptions.PluginPath}TessData\\", parameters[1], OcrEngineMode.TesseractOnly)
                                             : parameters.Length == 3
                                                 ? new Tesseract(
                                                     $"{PluginOptions.PluginPath}TessData\\", parameters[1], OcrEngineMode.TesseractOnly,
                                                     parameters[2]
                                                 )
                                                 : null) {
                if (tesseract == null) {
                    ar.setError("Failed to initialize recognizer due to invalid number of parameters.");
                    grayImage.Dispose();
                    return ar;
                }

                string recognizedText;
                using (Image<Gray, byte> imgThold = grayImage) {
                    CvInvoke.Threshold(grayImage, imgThold, 140, 255, ThresholdType.Binary);
                    tesseract.SetImage(imgThold);
                    recognizedText = tesseract.GetUTF8Text();
                }
                if (string.IsNullOrWhiteSpace(recognizedText)) {
                    ar.setError("No recognized symbols.");
                }
                else {
                    ar.setSuccess(recognizedText);
                }
                grayImage.Dispose();
            }
            return ar;
        }

        private actionResult TrainFace(string[] parameters) {
            var ar = new actionResult();

            #region Parameters parsing

            switch (parameters.Length) {
                case 0: {
                    ar.setError("Path to image not specified.");
                    return ar;
                }
                case 1: {
                    ar.setError("Face name not specified.");
                    return ar;
                }
            }

            Image<Gray, byte> grayImage;
            if (string.IsNullOrEmpty(parameters[0])) {
                using (var capture = new VideoCapture()) {
                    Thread.Sleep(PluginOptions.CameraDelayMs);
                    grayImage = capture.QueryFrame().ToImage<Gray, byte>();
                }
            }
            else {
                try {
                    grayImage = new Image<Gray, byte>(parameters[0]);
                }
                catch {
                    ar.setError("Invalid path to image.");
                    return ar;
                }
            }

            if (PluginOptions.UseImageCorrection) {
                grayImage._EqualizeHist();
            }

            #endregion

            Rectangle[] faces;
            using (var classifier = new CascadeClassifier($"{PluginOptions.CascadesPath}haarcascade_frontalface_default.xml")) {
                faces = classifier.DetectMultiScale(grayImage, 1.1, 10);
            }
            if (faces.Length == 0) {
                ar.setError("No face recognized.");
                return ar;
            }
            using (Image<Gray, byte> faceImage = grayImage.Copy(faces[0]).Resize(100, 100, Inter.Cubic)) {
                MainCtl.TrainedImages.Add(faceImage);
                PluginOptions.PeopleFaces.Add(PluginOptions.PeopleFaces.Count + 1, parameters[1]);
                faceImage.Save($"{PluginOptions.PluginPath}Faces\\face{MainCtl.TrainedImages.Count}.bmp");
            }

            PluginOptions.SaveOptionsToXml();
            grayImage.Dispose();

            using (FaceRecognizer recognizer = new EigenFaceRecognizer()) {
                recognizer.Train(MainCtl.TrainedImages.ToArray(), PluginOptions.PeopleFaces.Keys.ToArray());
                recognizer.Write($"{PluginOptions.PluginPath}SavedCascade.xml");
            }
            ar.setInfo($"Added face with name: {parameters[0]}.");
            return ar;
        }

        private actionResult RecognizeFace(string[] parameters) {
            var ar = new actionResult();
            if (MainCtl.TrainedImages.Count == 0) {
                ar.setError("Database contains no trained faces.");
                return ar;
            }

            #region Parameters parsing

            switch (parameters.Length) {
                case 0: {
                    ar.setError("Path to image not specified.");
                    return ar;
                }
                case 1: {
                    ar.setError("Face name not specified.");
                    return ar;
                }
            }

            Image<Gray, byte> grayImage;
            if (string.IsNullOrEmpty(parameters[0])) {
                using (var capture = new VideoCapture()) {
                    Thread.Sleep(PluginOptions.CameraDelayMs);
                    grayImage = capture.QueryFrame().ToImage<Gray, byte>();
                }
            }
            else {
                try {
                    grayImage = new Image<Gray, byte>(parameters[0]);
                }
                catch {
                    ar.setError("Invalid path to image.");
                    return ar;
                }
            }

            if (PluginOptions.UseImageCorrection) {
                grayImage._EqualizeHist();
            }

            #endregion

            Rectangle[] faces;
            using (var classifier = new CascadeClassifier(PluginOptions.CascadesPath + "haarcascade_frontalface_default.xml")) {
                faces = classifier.DetectMultiScale(grayImage, 1.1, 10);
            }
            if (faces.Length == 0) {
                ar.setError("No trained faces found.");
                return ar;
            }

            var resultString = "";
            foreach (Rectangle face in faces) {
                using (FaceRecognizer recognizer = new EigenFaceRecognizer()) {
                    recognizer.Read(PluginOptions.PluginPath + "SavedCascade.xml");
                    FaceRecognizer.PredictionResult recoResult = recognizer.Predict(grayImage.Copy(face).Resize(100, 100, Inter.Cubic));
                    resultString += $"<{PluginOptions.PeopleFaces.ElementAt(recoResult.Label)}:{recoResult.Distance}>";
                }
            }
            grayImage.Dispose();
            ar.setSuccess(resultString);
            return ar;
        }

        //private void MotionDetector() {
        //    VideoCapture capture = new VideoCapture();
        //    Thread.Sleep(PluginOptions.CameraDelayMs);
        //    Image<Gray, byte> lastFrame = capture.QueryFrame().ToImage<Gray, byte>();
        //    capture.Dispose();
        //    Image<Gray, byte> diffFrame = grayFrame.AbsDiff(lastFrame);
        //    int moves = diffFrame.CountNonzero()[0];

        //    if (moves > threshold) {

        //    }

        //}

        private static void SaveImage(Bitmap imageFrame) {
            //using (var tmpStrm = new MemoryStream()) {
            //    imageFrame.Save(tmpStrm, ImageFormat.Png);
            //}
            imageFrame.Save(PluginOptions.PluginPath + "Image.png", ImageFormat.Png);
        }

        #endregion
    }
}
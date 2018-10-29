using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Timer = System.Windows.Forms.Timer;

namespace Vision {
    public partial class CtlMain : UserControl {
        public readonly List<Image<Gray, byte>> TrainedImages = new List<Image<Gray, byte>>();

        public CtlMain() {
            InitializeComponent();
            NumCamDelay.Text           = PluginOptions.CameraDelayMs.ToString();
            UseColorCorrection.Checked = PluginOptions.UseImageCorrection;
        }

        private void btSave_Click(object sender, EventArgs e) {
            // receive options from viewv
            PluginOptions.CameraDelayMs      = Convert.ToInt32(NumCamDelay.Text);
            PluginOptions.UseImageCorrection = UseColorCorrection.Checked;
            // save options
            try {
                PluginOptions.SaveOptionsToXml();

                // show that settings have been saved
                Color  prevBtBackColor = btSave.BackColor;
                string prevBtMessage   = btSave.Text;
                btSave.Enabled   = false;
                btSave.BackColor = Color.FromArgb(202, 81, 0);
                btSave.Text      = "Options saved.";
                var timer = new Timer { Interval = 2500 };
                timer.Tick += delegate {
                    btSave.BackColor = prevBtBackColor;
                    btSave.Text      = prevBtMessage;
                    btSave.Enabled   = true;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex) {
                MessageBox.Show("An error occurred while saving options:\r\n" + ex);
            }
        }

        private void CtlMain_Load(object sender, EventArgs e) {
            using (var capture = new VideoCapture()) {
                Thread.Sleep(PluginOptions.CameraDelayMs);
                TestImage.Image = capture.QueryFrame();
            }
        }

        private void Header_Click(object sender, EventArgs e) {
            Process.Start(PluginOptions.PluginPath);
        }

        private void BtnSnapshot_Click(object sender, EventArgs e) {
            using (var capture = new VideoCapture()) {
                Thread.Sleep(PluginOptions.CameraDelayMs);
                using (Image<Bgr, byte> imageFrame = capture.QueryFrame().ToImage<Bgr, byte>()) {
                    if (imageFrame == null) {
                        return;
                    }
                    using (Image<Gray, byte> grayImage = imageFrame.Convert<Gray, byte>()) {
                        if (PluginOptions.UseImageCorrection) {
                            grayImage._EqualizeHist();
                        }
                        using (var classifier = new CascadeClassifier(PluginOptions.CascadesPath + "haarcascade_frontalface_default.xml")) {
                            Rectangle[] part1 = classifier.DetectMultiScale(grayImage, 1.1, 10);
                            if (part1.Length == 0) {
                                return;
                            }
                            LabelFacesList.Text = "";
                            foreach (Rectangle face in part1) {
                                using (Image<Gray, byte> resultingImage = imageFrame.Copy(face).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic)) {
                                    if (PluginOptions.UseImageCorrection) {
                                        resultingImage._EqualizeHist();
                                    }
                                    imageFrame.Draw(face, new Bgr(Color.Blue), 2);
                                    TestImage.Image = imageFrame;
                                    if (TrainedImages.Count == 0) {
                                        continue;
                                    }
                                    using (FaceRecognizer recognizer = new EigenFaceRecognizer()) {
                                        recognizer.Read(PluginOptions.PluginPath + "SavedCascade.xml");
                                        LabelFacesList.Text +=
                                            $"{PluginOptions.PeopleFaces.ElementAt(recognizer.Predict(resultingImage).Label).Value}\r\n";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DetectAndTrain_Click(object sender, EventArgs e) {
            using (var capture = new VideoCapture()) {
                Thread.Sleep(PluginOptions.CameraDelayMs);
                using (Image<Bgr, byte> imageFrame = capture.QueryFrame().ToImage<Bgr, byte>()) {
                    if (imageFrame == null) {
                        return;
                    }
                    using (Image<Gray, byte> grayframe = imageFrame.Convert<Gray, byte>()) {
                        if (PluginOptions.UseImageCorrection) {
                            grayframe._EqualizeHist();
                        }
                        Rectangle[] part1;
                        using (var classifier = new CascadeClassifier(PluginOptions.CascadesPath + "haarcascade_frontalface_default.xml")) {
                            part1 = classifier.DetectMultiScale(grayframe, 1.1, 10);
                        }
                        if (part1.Length > 0) {
                            Rectangle face = part1[0]; //выбор первого лица
                            using (Image<Gray, byte> resultingImage = imageFrame.Copy(face).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic)) {
                                imageFrame.Draw(face, new Bgr(Color.Blue), 2);
                                TestImage.Image        = imageFrame;
                                DetectedGrayFace.Image = resultingImage;

                                #region Добавление лица

                                if (string.IsNullOrEmpty(FaceName.Text)) {
                                    MessageBox.Show(
                                        "Сначала введите имя распознанного лица", "Имя лица не указано", MessageBoxButtons.OK, MessageBoxIcon.Warning
                                    );
                                    return;
                                }
                                TrainedImages.Add(resultingImage);
                                PluginOptions.PeopleFaces.Add(
                                    PluginOptions.PeopleFaces.Count + 1,
                                    FaceName.Text
                                );
                                TrainedImages.Last().Save($"{PluginOptions.PluginPath}Faces\\face{TrainedImages.Count}.bmp");

                                #endregion
                            }
                            PluginOptions.SaveOptionsToXml();

                            using (FaceRecognizer recognizer = new EigenFaceRecognizer()) {
                                recognizer.Train(TrainedImages.ToArray(), PluginOptions.PeopleFaces.Keys.ToArray());
                                recognizer.Write(PluginOptions.PluginPath + "SavedCascade.xml");
                            }
                            MessageBox.Show("Лицо успешно добавлено", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else {
                            MessageBox.Show("лиц не найдено", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
    }
}
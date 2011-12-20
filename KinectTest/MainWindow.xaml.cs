using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Research.Kinect.Nui;
using Microsoft.Research.Kinect.Audio;
using Coding4Fun.Kinect.Wpf;

namespace KinectTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nui.Initialize(RuntimeOptions.UseSkeletalTracking);

            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            nui.SkeletonEngine.TransformSmooth = true;
            TransformSmoothParameters parameters = new TransformSmoothParameters();
            parameters.Smoothing = 0.3f;
            parameters.Correction = 0.3f;
            parameters.Prediction = 0.4f;
            parameters.JitterRadius = 1.0f;
            parameters.MaxDeviationRadius = 0.5f;
            nui.SkeletonEngine.SmoothParameters = parameters;
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame allSkeletons = e.SkeletonFrame;

            //get the first tracked skeleton
            SkeletonData skeleton = (from s in allSkeletons.Skeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();

            //Joint HandRight = skeleton.Joints[JointID.HandRight].ScaleTo(640, 480);
            if (skeleton != null)
            {
                SetEllipsePosition(headEllipse, skeleton.Joints[JointID.Head]);
                SetEllipsePosition(leftHandEllipse, skeleton.Joints[JointID.HandLeft]);
                SetEllipsePosition(rightHandEllipse, skeleton.Joints[JointID.HandRight]);
                SetEllipsePosition(leftElbowEllipse, skeleton.Joints[JointID.ElbowLeft]);
                SetEllipsePosition(rightElbowEllipse, skeleton.Joints[JointID.ElbowRight]);
                SetEllipsePosition(shoulderCenterEllipse, skeleton.Joints[JointID.ShoulderCenter]);
                SetEllipsePosition(shoulderLeftEllipse, skeleton.Joints[JointID.ShoulderLeft]);
                SetEllipsePosition(shoulderRightEllipse, skeleton.Joints[JointID.ShoulderRight]);
                SetEllipsePosition(hipLeftEllipse, skeleton.Joints[JointID.HipLeft]);
                SetEllipsePosition(hipRightEllipse, skeleton.Joints[JointID.HipRight]);
                SetEllipsePosition(kneeLeftEllipse, skeleton.Joints[JointID.KneeLeft]);
                SetEllipsePosition(kneeRightEllipse, skeleton.Joints[JointID.KneeRight]);
            }
        }

        private void SetEllipsePosition(FrameworkElement ellipse, Joint joint)
        {
            var scaledJoint = joint.ScaleTo(1280, 1024, .5f, .5f);

            Canvas.SetLeft(ellipse, scaledJoint.Position.X);
            Canvas.SetTop(ellipse, scaledJoint.Position.Y);
        }

        Runtime nui = new Runtime();
    }
}

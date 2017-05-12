using System;
using CoreGraphics;
using AssetsLibrary;
using UIKit;
using Foundation;
using CoreImage;

/*
"Sunrise near Atkeison Plateau" © 2012 Charles Atkeison, 
used under a Creative Commons Attribution-ShareAlike license: http://creativecommons.org/licenses/by-sa/3.0/
*/

namespace ColorControl {

    public class ImageViewController : UIViewController {
        
        UIImage sourceImage, displayImage;
        NSDictionary sourceMeta;

        UIButton cameraButton, saveButton, resetButton;
        UIImageView imageView;
        UISlider sliderC, sliderS, sliderB;
        UILabel labelC, labelS, labelB;



        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            Title = "Color Controls Pro";
            View.BackgroundColor = UIColor.White;

            var sz = this.View.Bounds;

            cameraButton = UIButton.FromType (UIButtonType.RoundedRect);
            cameraButton.Frame = new CGRect(10, 80, 90, 40);
            cameraButton.SetTitle ("Take Photo", UIControlState.Normal);
            cameraButton.TouchUpInside += (sender, e) => {
            
                TweetStation.Camera.TakePicture (this, (obj) =>{
                    // https://developer.apple.com/library/ios/#documentation/uikit/reference/UIImagePickerControllerDelegate_Protocol/UIImagePickerControllerDelegate/UIImagePickerControllerDelegate.html#//apple_ref/occ/intfm/UIImagePickerControllerDelegate/imagePickerController:didFinishPickingMediaWithInfo:
                    var photo = obj.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
                    var meta = obj.ValueForKey(new NSString("UIImagePickerControllerMediaMetadata")) as NSDictionary;
            
                    sourceImage = photo;
                    displayImage = sourceImage.Scale(new CGSize(300, 200));
                    imageView.Image = displayImage;
                    sourceMeta = meta;

                    sliderS.Value = 1;
                    sliderB.Value = 0;
                    sliderC.Value = 1;
                });
            };
            View.Add (cameraButton);

            resetButton = UIButton.FromType (UIButtonType.RoundedRect);
            resetButton.Frame = new CGRect(110, 80, 90, 40);
            resetButton.SetTitle ("Reset", UIControlState.Normal);
            resetButton.TouchUpInside += (sender, e) => {
                sliderS.Value = 1;    
                sliderB.Value = 0;
                sliderC.Value = 1;

                HandleValueChanged (sender, e);
            };
            View.Add (resetButton);


            // ACCESS DENIED
            //saveButton = UIButton.FromType (UIButtonType.RoundedRect);
            //saveButton.Frame = new CGRect(210, 80, 90, 40);
            //saveButton.SetTitle ("Save", UIControlState.Normal);
            //saveButton.TouchUpInside += (sender, e) => {
            //    ALAssetsLibrary library = new ALAssetsLibrary();
                
            //    var img = AdjustImage (sourceImage);
                
            //    if (sourceMeta == null) sourceMeta = new NSDictionary(); // when using 'clouds.jpg'

            //    library.WriteImageToSavedPhotosAlbum (img.CGImage, sourceMeta, (assetUrl, error) => {
            //        Console.WriteLine ("SAVED TO assetUrl:"+assetUrl);

   //                 new UIAlertView("Saved", "Photo saved to Camera Roll" + error, null, "OK", null).Show ();
            //    });
            //};
            //View.Add (saveButton);    
            
            labelC = new UILabel(new CGRect(10, 240, 90, 20));
            labelS = new UILabel(new CGRect(10, 200, 90, 20));
            labelB = new UILabel(new CGRect(10, 160, 90, 20));

            labelC.Text = "Contrast";
            labelS.Text = "Saturation";
            labelB.Text = "Brightness";

            View.Add (labelC);
            View.Add (labelS);
            View.Add (labelB);

            sliderB = new UISlider(new CGRect(100, 160, 210, 20));
            sliderS = new UISlider(new CGRect(100, 200, 210, 20));
            sliderC = new UISlider(new CGRect(100, 240, 210, 20));
            
            // http://developer.apple.com/library/mac/#documentation/graphicsimaging/reference/CoreImageFilterReference/Reference/reference.html#//apple_ref/doc/filter/ci/CIColorControls
            sliderS.MinValue = 0;
            sliderS.MaxValue = 2;
            sliderS.Value = 1;
            
            sliderB.MinValue = -1;
            sliderB.MaxValue = 1;
            sliderB.Value = 0;

            sliderC.MinValue = 0;
            sliderC.MaxValue = 4;
            sliderC.Value = 1;
            
            // update the image in 'real time' as the sliders are moved
            sliderC.ValueChanged += HandleValueChanged;
            sliderS.ValueChanged += HandleValueChanged;
            sliderB.ValueChanged += HandleValueChanged;

            View.Add (sliderC);
            View.Add (sliderS);
            View.Add (sliderB);

            //imageView = new UIImageView(new CGRect(10, 240, 300, 200));
            imageView = new UIImageView(new CGRect(0, 290, sz.Width, sz.Width / 3 * 2));
            //sourceImage = UIImage.FromFile ("clouds.jpg");
            //displayImage = sourceImage;
            //imageView.Image = displayImage;
            View.Add (imageView);

            if (!UIImagePickerController.IsSourceTypeAvailable (UIImagePickerControllerSourceType.Camera)) {
                cameraButton.SetTitle ("No camera", UIControlState.Disabled);
                cameraButton.SetTitleColor (UIColor.Gray, UIControlState.Disabled);
                cameraButton.Enabled = false;
            }
        }

        void HandleValueChanged(object sender, EventArgs e)
        {    // use the low-res version
            if (displayImage != null)
            { 
                imageView.Image = AdjustImage(displayImage);
            }
        }

        CIContext context;
        CIColorControls colorCtrls; //CIFilter

        UIImage AdjustImage (UIImage image) 
        {
            if (colorCtrls == null)
                colorCtrls = new CIColorControls () { Image = CIImage.FromCGImage (image.CGImage) };
            else
                colorCtrls.Image = CIImage.FromCGImage(image.CGImage);

            if (context == null)
                context = CIContext.FromOptions (null);

            colorCtrls.Brightness = sliderB.Value; 
            colorCtrls.Saturation = sliderS.Value; 
            colorCtrls.Contrast = sliderC.Value;

            using (var outputImage = colorCtrls.OutputImage) {
                var result = context.CreateCGImage (outputImage, outputImage.Extent);
                return UIImage.FromImage (result);
            }
        }
    }
}
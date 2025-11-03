using PinHoard.viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace PinHoard.view
{
    public class ColourPicker : Grid
    {
        public ColourPickerViewModel context;
        private ColourField r, g, b;

        public BoardViewModel bvm
        {
            get => (BoardViewModel)GetValue(bvmProperty);
            set => SetValue(bvmProperty, value);
        }
        public static readonly DependencyProperty bvmProperty =
            DependencyProperty.Register(nameof(bvm), typeof(BoardViewModel), typeof(ColourPicker));

        private string GetHex() { return (r.text + g.text + b.text); }
        public ColourPicker(BoardViewModel bvm)
        {
            this.bvm = bvm;
            this.context = new(bvm);
            this.Height = 55;
            this.Width = 200;
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEFEFEF"));

            this.Children.Add(r = new ColourField("R", HorizontalAlignment.Left));
            this.Children.Add(g = new ColourField("G", HorizontalAlignment.Center));
            this.Children.Add(b = new ColourField("B", HorizontalAlignment.Right));

            this.Children.Add(
                new Button
                {
                    Content = "Apply",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Width = 75,
                    Height = 18,
                    Margin = new Thickness(2),
                    Command = context.ApplyColourChange,
                    CommandParameter = GetHex()
                }
            );

            context.PropertyChanged += SetContainers;
        }
        public void SetContainers(object obj, PropertyChangedEventArgs e)
        {
            r.text = context.r;
            g.text = context.g;
            b.text = context.b;
        }
}

    internal class ColourField : Grid
    {
        private TextBox amt;
        public string text
        {
            get { return amt.Text; }
            set { amt.Text = value; }
        }
        public ColourField(string fieldName, HorizontalAlignment H_A)
        {
            this.VerticalAlignment = VerticalAlignment.Top;
            this.HorizontalAlignment = H_A;
            this.Margin = new Thickness(5); 
            this.Width = 5;

            Label label = new Label { 
                Content = $"{fieldName}:",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            amt = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 35
            };

            this.Children.Add(label);
            this.Children.Add(amt);
        }
    }
}

//< Grid x: Name = "ColourPickerContainer" HorizontalAlignment = "Center" VerticalAlignment = "Top" Margin = "170,30,0,0" Height = "55" Width = "200" Background = "#FFEFEFEF" >
//    < !--< Grid x: Name = "HuePicker" Margin = "5,5" >
//        < Image x: Name = "HueBackground" Margin = "0,0" Height = "16" VerticalAlignment = "Top" Source = "images/test_rainbow.png" Stretch = "Fill" />
//        < Button x: Name = "HueSliderKnob" Height = "18" Width = "5" VerticalAlignment = "Top" Margin = "-1" />
//    </ Grid > -->
//    < !--< Grid x: Name = "TonePicker" Margin = "5,26" >
//        < Border x: Name = "ToneBackground" Height = "16" VerticalAlignment = "Top" >
//            < Border.Background >
//                < LinearGradientBrush EndPoint = "0.5,1" StartPoint = "0.5,0" >
//                    < LinearGradientBrush.RelativeTransform >
//                        < TransformGroup >
//                            < ScaleTransform CenterY = "0.5" CenterX = "0.5" />
//                            < SkewTransform CenterX = "0.5" CenterY = "0.5" />
//                            < RotateTransform Angle = "270" CenterX = "0.5" CenterY = "0.5" />
//                            < TranslateTransform />
//                        </ TransformGroup >
//                    </ LinearGradientBrush.RelativeTransform >
//                    < GradientStop Color = "Black" />
//                    < GradientStop Color = "White" Offset = "1" />
//                </ LinearGradientBrush >
//            </ Border.Background >
//        </ Border >
//        < Button x: Name = "ToneSliderKnob" Height = "18" Width = "5" VerticalAlignment = "Top" Margin = "-1" />
//    </ Grid > -->
//</ Grid >
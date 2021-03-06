﻿#pragma checksum "..\..\PrintQueueControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9F7E82934874F3DA97A7319033E3EB2D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34014
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using InstagramPatterns.InstagramApi;
using InstagramPrint;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace InstagramPrint {
    
    
    /// <summary>
    /// PrintQueueControl
    /// </summary>
    public partial class PrintQueueControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 27 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ImpCountBox;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PrintedCountBox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CanseledCountBox;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ImputImages;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView PrintedImages;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\PrintQueueControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView CanseledImages;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/InstagramPrint;component/printqueuecontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PrintQueueControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ImpCountBox = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.PrintedCountBox = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.CanseledCountBox = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 33 "..\..\PrintQueueControl.xaml"
            this.SearchTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ImputImages = ((System.Windows.Controls.ListView)(target));
            
            #line 36 "..\..\PrintQueueControl.xaml"
            this.ImputImages.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Images_Selected);
            
            #line default
            #line hidden
            return;
            case 7:
            this.PrintedImages = ((System.Windows.Controls.ListView)(target));
            
            #line 65 "..\..\PrintQueueControl.xaml"
            this.PrintedImages.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Images_Selected);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CanseledImages = ((System.Windows.Controls.ListView)(target));
            
            #line 92 "..\..\PrintQueueControl.xaml"
            this.CanseledImages.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Images_Selected);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 59 "..\..\PrintQueueControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ImputImagesItemButtonClick);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 86 "..\..\PrintQueueControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PrintedQueueItemButtonClick);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 113 "..\..\PrintQueueControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CanseledQueueItemButtonClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}


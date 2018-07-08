namespace AccesoPrivadaLaurel.Droid
open System

open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS
open Xamarin.Forms.Platform.Android

type Resources = AccesoPrivadaLaurel.Droid.Resource

[<Activity (Label = "AccesoPrivadaLaurel.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type MainActivity() =
    inherit FormsAppCompatActivity()

    override this.OnCreate (bundle: Bundle) =
        let mTelephonyMgr = downcast Application.Context.GetSystemService(Android.Content.Context.TelephonyService) : Android.Telephony.TelephonyManager
        let number = mTelephonyMgr.Line1Number

        FormsAppCompatActivity.TabLayoutResource <- Resources.Layout.Tabbar
        FormsAppCompatActivity.ToolbarResource <- Resources.Layout.Toolbar

        base.OnCreate (bundle)

        Xamarin.Forms.Forms.Init (this, bundle)

        this.LoadApplication (new AccesoPrivadaLaurel.App (number))

namespace AccesoPrivadaLaurel.Droid
open System

open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.Net.Wifi
open Android.OS
open Android.Telephony
open Xamarin.Forms.Platform.Android

type Resources = AccesoPrivadaLaurel.Droid.Resource

[<Activity (Label = "AccesoPrivadaLaurel.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type MainActivity() =
    inherit FormsAppCompatActivity()

    override this.OnCreate (bundle: Bundle) =
        let SSID = "privadalaurel"
        let PASSWORD = "dixukAl0"

        let mTelephonyMgr = downcast Application.Context.GetSystemService(Android.Content.Context.TelephonyService) : TelephonyManager
        let number = mTelephonyMgr.Line1Number

        let connect () =
            let wifiManager = downcast Android.App.Application.Context.GetSystemService(Context.WifiService) : WifiManager
            let SSID' = wifiManager.ConnectionInfo.SSID

(*            let formattedSsid = "\"{" + SSID + "\"}"
            let formattedPassword = "\"{" + PASSWORD + "\"}"
            let wifiConfig = new WifiConfiguration(Ssid = formattedSsid,
                                                   PreSharedKey = formattedPassword)
            let formattedSsid = "\"{" + SSID + "\"}"
            let formattedPassword = "\"{" + PASSWORD + "\"}"
            let wifiConfig = new WifiConfiguration(Ssid = formattedSsid,
                                                   PreSharedKey = formattedPassword)

            let addNetwork = wifiManager.AddNetwork(wifiConfig)

            match wifiManager.ConfiguredNetworks.[addNetwork] with
            | null -> ()
            | network -> wifiManager.Disconnect() |> ignore
                         let enableNetwork = wifiManager.EnableNetwork(network.NetworkId, true)
                         ()*)
            ()

        FormsAppCompatActivity.TabLayoutResource <- Resources.Layout.Tabbar
        FormsAppCompatActivity.ToolbarResource <- Resources.Layout.Toolbar

        base.OnCreate (bundle)

        Xamarin.Forms.Forms.Init (this, bundle)

        this.LoadApplication (new AccesoPrivadaLaurel.App (number))

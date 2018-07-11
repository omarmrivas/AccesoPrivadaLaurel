namespace AccesoPrivadaLaurel

open Xamarin.Forms

type App(number : string, connect : unit -> unit) =
    inherit Application(MainPage = MainPage(number, connect))

//    do Application.Current.Properties.["PHONE"] <- number


namespace AccesoPrivadaLaurel

open Xamarin.Forms

type App(number : string) =
    inherit Application(MainPage = MainPage(number))

//    do Application.Current.Properties.["PHONE"] <- number


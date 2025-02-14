// JavaScript source code

// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.OptionSet) == "undefined") {
    Endeavor.Skanetrafiken.OptionSet = {

        // #region Generated Option Sets
        account_statuscode:{
            Aktiv: 1,
            Inaktiv: 2
        },

        lead_leadsourcecode:{
            GAN: 100000000,
            SPAR: 100000001,
            Ovrig: 100000002,
            MittKonto: 899310000,
            LaddaKort: 899310001,
            OinloggatKop: 899310002
        },

        processstage_category:{
            Kvalificera: 0,
            Tafram: 1,
            Foresla: 2,
            Stang: 3,
            Identifiera: 4,
            Efterforskning: 5,
            Los: 6
        }
        // #endregion
    };
}
if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }


// *******************************************************
// Form script functions
// *******************************************************


CGISweden.formscriptfunctions =
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Add a onchange function to a field. 
    SetOnChangeFunction: function (name, functionname, formContext) {
        try {
            formContext.data.entity.attributes.get(name).addOnChange(functionname);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetOnChangeFunction\n\n" + e.Message);
        }
    },

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Save entity
    SaveEntity: function (formContext) {
        try {
            formContext.data.entity.save();
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SaveEntity\n\n" + e.Message);
        }
    },

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Save and close entity
    SaveAndCloseEntity: function (formContext) {
        try {
            formContext.data.entity.save("saveandclose");
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SaveAndCloseEntity\n\n" + e.Message);
        }
    },

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get objectid of entity
    GetObjectID: function (formContext) {
        try {
            return formContext.data.entity.getId();
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetObjectID\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get formtype.
    GetFormType: function (formContext) {
        var _returnvalue = null;
        try {
            _returnvalue = formContext.ui.getFormType();
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetFormType\n\n" + e.Message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Hide or display tab.
    HideOrDisplayTab: function (name, visible, formContext) {
        try {
            formContext.ui.tabs.get(name).setVisible(visible);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.HideOrDisplayTab\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //expanded or collapsed
    SetDisplayState: function (name, state, formContext) {
        try {
            formContext.ui.tabs.get(name).setDisplayState(state);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetDisplayState\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Hide or display section.
    HideOrDisplaySection: function (tabname, sectionname, visible, formContext) {
        try {
            formContext.ui.tabs.get(tabname).sections.get(sectionname).setVisible(visible)
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.HideOrDisplaySection\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Hide or display field.
    HideOrDisplayField: function (name, visible, formContext) {
        try {
            formContext.getControl(name).setVisible(visible);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.HideOrDisplayField\n\n" + e.Message + name);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set value to a field
    SetValue: function (name, value, formContext) {
        try {
            formContext.data.entity.attributes.get(name).setValue(value);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetValue\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get value from a field
    GetValue: function (name, formContext) {
        var _returnvalue = null;
        try {
            _returnvalue = formContext.data.entity.attributes.get(name).getValue();
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetValue\n\n" + e.Message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set field enabled or disabled.
    SetState: function (name, state, formContext) {
        try {
            var _field = formContext.ui.controls.get(name);
            _field.setDisabled(state);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetState\n\n" + e.Message);
        }
    },

    SetSubmitModeAlways: function (name, formContext) {
        try {
            formContext.getAttribute(name).setSubmitMode("always");
        } catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetSubmitModeAlways\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get control state. Enabled or disabled
    GetState: function (name, formContext) {
        var _returnvalue = null;
        try {
            _returnvalue = formContext.getControl(name).getDisabled();
        } catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetState\n\n" + e.Message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // none, required, recommended
    SetRequiredLevel: function (name, state, formContext) {
        try {
            formContext.getAttribute(name).setRequiredLevel(state);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetRequiredLevel\n\n" + e.Message + name);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set submitmode for field
    // always, never, dirty
    SetSubmitMode: function (name, state, formContext) {
        try {
            formContext.data.entity.attributes.get(name).setSubmitMode(state);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetSubmitMode\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set value to lookupfield.
    SetLookup: function (attributeName, entityType, id, name, formContext) {
        try {
            var _attribute = formContext.getAttribute(attributeName);
            var _item = new Object();
            _item.id = id;
            _item.name = name;
            _item.entityType = entityType;
            var _lookup = new Array();
            _lookup[0] = _item;
            _attribute.setValue(_lookup);
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetLookup\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get id value from lookupfield.
    GetLookupid: function (name, formContext) {
        var _returnvalue = null;
        try {
            var _id = null;
            var _attribute = formContext.getAttribute(name);
            var _lookup = _attribute.getValue();
            if (_lookup != null)
                _id = _lookup[0].id;
            return _id;
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetLookupid\n\n" + e.Message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get name value from lookupfield.
    GetLookupName: function (name, formContext) {
        var _returnvalue = null;
        try {
            var _idName = null;
            var _attribute = formContext.getAttribute(name);
            var _lookup = _attribute.getValue();
            if (_lookup != null)
                _idName = _lookup[0].name;
            return _idName;
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetLookupName\n\n" + e.Message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set focus on field.
    SetFocusOnField: function (name, formContext) {
        try {
            formContext.ui.controls.get(name).setFocus();
        }
        catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.SetFocusOnField\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Check if field is disabled.
    GetDisabledField: function (arg, formContext) {
        try {
            return formContext.getControl(arg).getDisabled();
        } catch (e) {
            alert("Fel i CGISweden.formscriptfunctions.GetDisabledField\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Check if mandatory fields is not empty
    MandatoryPopulated: function (formContext) {
        populated = true;

        formContext.getAttribute(function (attribute, index) {
            if (attribute.getRequiredLevel() == "required") {
                if (attribute.getValue() === null) {
                    populated = false;
                }
            }
        });

        return populated;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //get date of today. Format YYYY-MM-DDTHH:MM:SS
    GetDateTime: function () {
        var now = new Date();
        var year = now.getFullYear();
        var month = now.getMonth() + 1;
        var day = now.getDate();
        var hour = now.getHours();
        var minute = now.getMinutes();
        var second = now.getSeconds();
        if (month.toString().length == 1) {
            var month = '0' + month;
        }
        if (day.toString().length == 1) {
            var day = '0' + day;
        }
        if (hour.toString().length == 1) {
            var hour = '0' + hour;
        }
        if (minute.toString().length == 1) {
            var minute = '0' + minute;
        }
        if (second.toString().length == 1) {
            var second = '0' + second;
        }
        var dateTime = year + '-' + month + '-' + day + 'T' + hour + ':' + minute + ':' + second;
        return dateTime;
    }

};

//
//
//
//
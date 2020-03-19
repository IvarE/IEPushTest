
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

// Common functions for CRM.
// Be very strict with new functions in this file

// Dependencies:
/// <reference path="SDK.Rest.js" />
/// <reference path="jquery-1.11.1.min.js" />

// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Common) == "undefined") {
    Endeavor.Common = {
    };
}

if (typeof (Endeavor.Common.Page) == "undefined") {
    Endeavor.Common.Page = {

        stopAutoSave: function (context) {
            /// <summary>
            /// Stop the auto save feature.
            /// Put on the OnSave event
            /// Use command: Endeavor.Common.Page.stopAutoSave and enable "Pass execution context as the first parameter"
            /// </summary>
            /// <param name="context"></param>
            var saveEvent = context.getEventArgs();
            if (saveEvent.getSaveMode() == 70 ||    // Form AutoSave Event
                saveEvent.getSaveMode() == 2) {     // Form AutoSave when form is closed.
                saveEvent.preventDefault(); //Stops the Save Event
            }
        },

        setLookup: function (fieldName, entRef) {
            /// <summary>
            /// Set value in a lookup field
            /// </summary>
            /// <param name="fieldName">The field to Set</param>
            /// <param name="entRef">Entity reference object</param>
            if (entRef != undefined || null) {
                var lookup = new Object();
                var lookupValue = new Array();
                if ("Id" in entRef) {
                    lookup.id = entRef.Id;
                }
                else if ("getId" in entRef) {
                    lookup.id = entRef.getId();
                }
                if ("LogicalName" in entRef) {
                    lookup.entityType = entRef.LogicalName;
                }
                else if ("getType" in entRef) {
                    lookup.entityType = entRef.getType();
                }
                if ("Name" in entRef) {
                    lookup.name = entRef.Name;
                }
                else if ("getName" in entRef) {
                    lookup.name = entRef.getName();
                }
                lookupValue[0] = lookup;
                Xrm.Page.getAttribute(fieldName).setValue(lookupValue);
            }
            else {
                Xrm.Page.getAttribute(fieldName).setValue();
            }
            // Problem with fields always beeing sent (Problem in Timereporting entity)
            //Xrm.Page.getAttribute(field).setSubmitMode("always");
        },

        getLookupId: function (fieldName) {
            /// <summary>
            /// Get Lookup field Id.
            /// </summary>
            /// <param name="fieldName">Field Name</param>
            var lookupid = null;
            var lookupObject = Xrm.Page.getAttribute(fieldName);

            if (lookupObject != null) {
                var lookUpObjectValue = lookupObject.getValue();
                if ((lookUpObjectValue != null)) {
                    lookupid = lookUpObjectValue[0].id;
                }
            }

            return lookupid;
        },

        getLookupName: function (fieldName) {
            /// <summary>
            /// Get Lookup field Name.
            /// </summary>
            /// <param name="fieldName">Field Name</param>
            var name = null;
            var lookupObject = Xrm.Page.getAttribute(fieldName);

            if (lookupObject != null) {
                var lookUpObjectValue = lookupObject.getValue();
                if ((lookUpObjectValue != null)) {
                    name = lookUpObjectValue[0].name;
                }
            }

            return name;
        },

        getLookupTypeName: function (fieldName) {
            /// <summary>
            /// Get Lookup Type Name (entity).
            /// </summary>
            /// <param name="fieldName">Field Name</param>
            var name = null;
            var lookupObject = Xrm.Page.getAttribute(fieldName);

            if (lookupObject != null) {
                var lookUpObjectValue = lookupObject.getValue();
                if ((lookUpObjectValue != null)) {
                    name = lookUpObjectValue[0].entityType;
                }
            }

            return name;
        },

        getSubGridSelectedRow: function (subGridName) {
            /// <summary>
            /// Get selected row ID from subGrid
            /// </summary>
            /// <param name="subGridName"></param>
            /// <returns type="">Returns a Row object .Id and .Name</returns>

            var grid = document.getElementById(subGridName).control;
            // Other way to get the control
            // grid = Xrm.Page.getControl("AgreementsToAddToJournal");
            // grid.$Y_0
            for (var rowNo = 0; rowNo < grid.get_selectedRecords().length; rowNo++) {
                //alert(grid.get_selectedRecords()[rowNo].Id);
                //alert(grid.get_selectedRecords()[rowNo].Name);
                return grid.get_selectedRecords()[rowNo];
            }
            return null;
        },

        convertToButton: function (fieldname, buttontext, buttonwidth, clickevent, title) {
            /// <summary>
            /// Convert a field to a button
            /// </summary>

            // CRM2013 Syntax
            if (document.getElementById(fieldname) != null) {
                var fieldId = "field" + fieldname;
                var width = "100px";                    // Standard width
                if (isNaN(buttonwidth) == false)
                    width = buttonwidth + "px";
                if (document.getElementById(fieldId) == null) {
                    var elementId = document.getElementById(fieldname + "_d");
                    var div = document.createElement("div");
                    div.style.width = width;
                    div.style.textAlign = "right";
                    div.style.display = "inline";
                    elementId.appendChild(div, elementId);
                    //div.innerHTML = '<button id="' + fieldId + '"  type="button" style="margin-left: 3px; width: 100%;" >' + buttontext + '</button>';
                    div.innerHTML = '<button id="' + fieldId + '"  type="button" style="margin-left: 3px;" >' + buttontext + '</button>';
                    document.getElementById(fieldname).style.width = "0%";
                    document.getElementById(fieldId).onclick = function () { clickevent(); };
                }
            }
        },

        initializeFormWait: function () {
            if (!jQuery.isFunction(jQuery.fn.center)) {
                jQuery.fn.center = function () {
                    this.css("position", "absolute");
                    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                                $(window).scrollTop()) + "px");
                    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                                $(window).scrollLeft()) + "px");
                    return this;
                }
            }

            var divContent = ['<div id="edp_formWaitDiv" style="background: url(/_imgs/AdvFind/progress.gif) no-repeat left #ffffca;background-position:10px; left: 650px; top: 162px; width: 300px; height: 100px; display: block; position: absolute; border:1px solid black; position: relative;z-index: 100;">',
            '<p id="edp_formWaitText" style="font: 18px/normal Segoe UI, Tahoma, Arial; text-align: left; font-size-adjust: none; font-stretch: normal; word-wrap: break-word; margin-left: 50px;">',
            'Working on it...',
            '</p>',
            '<a id="edp_formWaitHideLink" href="#" style="position: absolute; bottom: 0px; right: 0px;">hide</a>',
            '</div>'].join('');

            // Don't add div if it already exists.
            if ($('#edp_formWaitDiv').length <= 0) {
                $('body').append(divContent);
            }

            // center, and hide it initially 
            $('#edp_formWaitDiv').center().hide();

            $('#edp_formWaitHideLink').click(function () { Endeavor.Common.Page.hideFormWait(); return false; });
        },

        setFormWaitText: function (text) {
            $('#formWaitText').text(text);
        },

        setFormWaitImage: function (imageUrl) {
            $('#edp_formWaitDiv').css('background', 'url(' + imageUrl + ') no-repeat center');
        },

        showFormWait: function () {
            if ($('#edp_formWaitDiv').length > 0) {
                $('#edp_formWaitDiv').show();
            }
        },

        hideFormWait: function () {
            if ($('#edp_formWaitDiv').length > 0) {
                $('#edp_formWaitDiv').hide();
            }
        }
    }
}
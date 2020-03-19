
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

// Common functions for CRM.
// Be very strict with new functions in this file

/// <reference path="vsdoc/XrmPage-vsdoc.js" />
/// <reference path="Endeavor.Common.Data.js" />

if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Common) == "undefined") {
    Endeavor.Common = {
    };
}

if (typeof (Endeavor.Common.SaveAndCopyEntity) == "undefined") {
    Endeavor.Common.SaveAndCopyEntity = {

        getCookieName: function () {
            var cookieName = 'SaveAndNew_' + Xrm.Page.data.entity.getEntityName();
            return cookieName;
        },

        clearCookie: function () {
            Endeavor.Common.Data.createCookie(Endeavor.Common.SaveAndCopyEntity.getCookieName(), "", -1);
        },

        saveAttributesInCookie: function () {
            var attributesToCopy = Endeavor.Common.SaveAndCopyEntity.getAttributesToCopy();

            var attributesAsString = JSON.stringify(attributesToCopy);
            Endeavor.Common.Data.createCookie(Endeavor.Common.SaveAndCopyEntity.getCookieName(), attributesAsString, null);
        },

        getAttributesToCopy: function () {
            // Get entered attributes, skip null values
            var attributesToCopy = new Array();
            var allAttributesToExclude = Endeavor.Common.SaveAndCopyEntity.getDefaultAttributesToExclude();

            Xrm.Page.data.entity.attributes.forEach(function (attribute, index) {

                // Check if attribute should be copied
                if (allAttributesToExclude.indexOf(attribute.getName()) == -1) {

                    var value = attribute.getValue();

                    if (value != null) {
                        var attr = new Object();
                        attr.name = attribute.getName();
                        attr.type = attribute.getAttributeType();

                        if (value != null && attribute.getAttributeType() == "lookup") {
                            var lookup = new Object();
                            lookup.id = value[0].id;
                            lookup.entityType = value[0].entityType;
                            lookup.name = value[0].name;
                            attr.value = new Array();
                            attr.value[0] = lookup;
                        }
                        else {
                            attr.value = value;
                        }

                        attributesToCopy.push(attr);
                    }
                }
            });

            return attributesToCopy;
        },


        getAttributesFromCookie: function () {
            var attributesAsString = Endeavor.Common.Data.readCookie(Endeavor.Common.SaveAndCopyEntity.getCookieName());

            if (attributesAsString != null && attributesAsString.length > 0) {
                // Clear cookie
                Endeavor.Common.SaveAndCopyEntity.clearCookie();
                // Return entity
                return JSON.parse(attributesAsString);
            }

            return null;
        },

        saveAndCopy: function () {
            // Save cookie
            Endeavor.Common.SaveAndCopyEntity.saveAttributesInCookie();

            // Save and new
            Xrm.Page.data.entity.save("saveandnew");
        },

        getDefaultAttributesToExclude: function () {
            var attributesToExclude = new Array('createdby'
                                            , 'createdon'
                                            , 'createdonbehalfby'
                                            , 'importsequencenumber'
                                            , 'modifiedby'
                                            , 'modifiedon'
                                            , 'modifiedonbehalfby'
                                            , 'overriddencreatedon'
                                            , 'ownerid'
                                            , 'owningbusinessunit'
                                            , 'owningteam'
                                            , 'owninguser'
                                            , 'statecode'
                                            , 'statuscode'
                                            , 'timezoneruleversionnumber'
                                            , 'utcconversiontimezonecode'
                                            , 'versionnumber'
                                            , 'id');
            return attributesToExclude;
        },

        fillAttributesFromCopy: function (includeAttributes, attributes, priorityAttributes) {
            /// <summary>
            /// Fill attributes saved in cookie 
            /// </summary>
            /// <param name="includeAttributes">Include (true) or Exclude (false) attributes</param>
            /// <param name="attributes">Include/Exclude attributes</param>
            /// <param name="priorityAttributes">Attributes to start with in sort order</param>
            var attributesFromCookie = Endeavor.Common.SaveAndCopyEntity.getAttributesFromCookie();
            if (attributesFromCookie != null) {

                // Get all attributes to exclude from copy
                var allAttributesToExclude = new Array();

                if (!includeAttributes) {
                    allAttributesToExclude.concat(Endeavor.Common.SaveAndCopyEntity.getDefaultAttributesToExclude());
                    if (attributes != null) {
                        allAttributesToExclude = allAttributesToExclude.concat(attributes);
                    }
                }
                // Loop priority attributes
                if (priorityAttributes != null) {
                    priorityAttributes.forEach(function (priorityAttributeName, index1) {

                        // Loop all attributes
                        attributesFromCookie.forEach(function (attribute, index2) {

                            // Check if attribute should be copied
                            if ((!includeAttributes && allAttributesToExclude.indexOf(attribute.name) == -1) || (includeAttributes && attributes.indexOf(attribute.name) != -1 && allAttributesToExclude.indexOf(attribute.name) == -1) &&
                                attribute.name == priorityAttributeName) {
                                // Set value
                                var attr = Xrm.Page.getAttribute(attribute.name);
                                if (attr != null) {
                                    allAttributesToExclude.push(attribute.name);
                                    attr.setValue(attribute.value);
                                    attr.fireOnChange();
                                }
                            }
                        });
                    });
                }

                // Loop all attributes
                attributesFromCookie.forEach(function (attribute, index1) {

                    // Check if attribute should be copied and is not in priority attributes
                    if ((!includeAttributes && allAttributesToExclude.indexOf(attribute.name) == -1) || (includeAttributes && attributes.indexOf(attribute.name) != -1 && allAttributesToExclude.indexOf(attribute.name) == -1)) {
                        // Set value
                        var attr = Xrm.Page.getAttribute(attribute.name);
                        if (attr != null) {
                            allAttributesToExclude.push(attribute.name);
                            attr.setValue(attribute.value);
                            attr.fireOnChange();
                        }
                    }
                });

                return true;
            }

            return false;
        },
    };
}

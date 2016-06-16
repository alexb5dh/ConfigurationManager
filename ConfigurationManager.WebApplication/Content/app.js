$(function() {
    var App = function(serviceUrl) {
        var $sectionContainer = $("#section-container");
        var $keyInput = $("#key-input");

        var readKey = function() {
            return $keyInput.val();
        }

        var displaySection = function(section) {
            for (var key in section) {
                var $keyElem = $("<h4></h4>")
                    .addClass("list-group-item-heading")
                    .text(key);
                var $valueElem = $("<div></div>")
                    .addClass("list-group-item-text")
                    .text(section[key]);
                var $pairElem = $("<div></div>")
                    .addClass("list-group-item")
                    .append($keyElem)
                    .append($valueElem);

                $sectionContainer.html($pairElem[0].outerHTML);
            }
        }

        var buildAlert = function(message) {
            var $closeBtn = $("<button type='button'></button>")
                .addClass("close")
                .attr("data-dismiss", "alert")
                .append($("<span></span>").text("×"));

            return $("<div></div>")
                .addClass("alert")
                .addClass("alert-dismissible")
                .append($closeBtn)
                .append(message);
        }

        var displaySuccess = function (message) {
            $("#status-container")
                .html(buildAlert(message)
                    .addClass("alert-success")
                    [0].outerHTML);
        }

        var displayError = function(message) {
            $("#status-container")
                .html(buildAlert(message)
                    .addClass("alert-danger")
                    [0].outerHTML);
        }

        this.getValue = function() {
            var key = readKey();
            $.ajax(serviceUrl + "/value?key=" + key)
                .success(function (value) {
                    var pair = {};
                    pair[key] = value;
                    displaySection(pair);
                    displaySuccess("GET value - OK!");
                }).error(function(xhr) {
                    displayError(JSON.parse(xhr.responseText).ExceptionMessage);
                });
        }

        this.getSection = function() {
            $.ajax(serviceUrl + "/section?key=" + readKey())
                .success(function(section) {
                    displaySection(section);
                    displaySuccess("GET section - OK!");
                }).error(function (xhr) {
                    displayError(JSON.parse(xhr.responseText).ExceptionMessage);
                });
        }
    }

    $.ajax("/app.json")
        .success(function(appSettings) {
            var serviceUrl = appSettings.connectionStrings["ConfigurationManager.WebService"];
            // Global - to simplify debugging
            window.app = new App(serviceUrl);
        });

    $("#action-selector").find("li a").click(function () {
        var actionName = $(this).data("action");
        app[actionName]();
    });
});
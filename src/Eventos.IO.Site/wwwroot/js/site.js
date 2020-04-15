// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ValidacoesEvento() {
    $.validator.methods.range = function (value, element, param) {
        var globalizeValue = value.replace(",", ".");
        return this.optional(element) || (globalizeValue >= param[0] && globalizeValue <= param[1]);
    }

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    }

    $('#DataInicio').datepicker({
        format: "dd/mm/yyyy",
        startDate: "tomorroy",
        language: "pt-br",
        orientation: "bottom right",
        autoclose: true
    });

    $('#DataFim').datepicker({
        format: "dd/mm/yyyy",
        startDate: "tomorroy",
        language: "pt-br",
        orientation: "bottom right",
        autoclose: true
    });
}
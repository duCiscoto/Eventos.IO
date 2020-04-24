﻿function ValidacoesEvento() {
    $.validator.methods.range = function (value, element, param) {
        var globalizeValue = value.replace(",", ".");
        return this.optional(element) || (globalizeValue >= param[0] && globalizeValue <= param[1]);
    }

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    }

    toastr.options = {
        "closeButton": false,
        "debug": true,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    $('#DataInicio').datepicker({
        format: "dd/mm/yyyy",
        startDate: "tomorrow",
        language: "pt-br",
        orientation: "bottom right",
        autoclose: true
    })
        .attr('type', 'text'); //Desabilita o datepicker nativo do navegador (Chrome)

    $('#DataFim').datepicker({
        format: "dd/mm/yyyy",
        startDate: "tomorrow",
        language: "pt-br",
        orientation: "bottom right",
        autoclose: true
    })
        .attr('type', 'text'); //Desabilita o datepicker nativo do navegador (Chrome)

    // Validações de exibição do endereço
    $(document).ready(function () {
        var $inputOnline = $("#Online");
        var $inputGratuito = $("#Gratuito");

        MostrarEndereco();
        MostrarValor();

        $inputOnline.click(function () {
            MostrarEndereco();
        })

        $inputGratuito.click(function () {
            MostrarValor();
        })

        function MostrarEndereco() {
            if ($inputOnline.is(":checked")) $("#EnderecoForm").hide();
            else $("#EnderecoForm").show();
        }

        function MostrarValor() {
            if ($inputGratuito.is(":checked")) {
                $("#Valor").val("0");
                $("#Valor").prop("disabled", true);
            }
            else {
                $("#Valor").prop("disabled", false);
            }
        }
    });
}

function AjaxModal() {
    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal({
                                keyboard: true
                            },
                                'show');
                            bindForm(this);
                        });
                    return false;
                });
        });

        function bindForm(dialog) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide');
                            $('#replacetarget').load(result.url); // Carrega o resultado HTML para a div demarcada
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
                return false;
            });
        }
    });
}
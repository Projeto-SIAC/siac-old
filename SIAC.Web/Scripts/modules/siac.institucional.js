﻿siac.Institucional = siac.Institucional || {};

siac.Institucional.Configurar = (function () {
	function iniciar() {

		//$('.menu .item').tab();
		$('.ui.dropdown').dropdown();
		$('.ui.accordion').accordion({ animateChildren: false });

		$('.tabular.menu .item').tab({
			history: true,
			historyType: 'state',
			path: '/institucional/configurar'
		});
	}

	return {
		iniciar: iniciar
	}
})();

siac.Institucional.Gerar = (function () {
    var _qteQuestoes = 0;

    function iniciar() {

        $('.ui.dropdown').dropdown();
        $('.tabular.menu .item').tab();
        $('h3').popup();
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.cancelar.button').popup({
            on: 'click',
            inline: true
        });

        //Definindo o Tipo Default como 1 ( Objetiva )
        $('#ddlTipo').dropdown('set selected', 2);

        //Adicionando Alternativas por default
        for (var i = 0; i < 5; i++) {
            adicionarAlternativa();
        }

        //Funções
        $('#ddlTipo').change(function () {
            mostrarCamposPorTipo();
        });

        $('.questao.objetiva .adicionar.button').click(function () {
            adicionarAlternativa();
        });

        $('.adicionar.questao.button').click(function () {
            verificar();
        })
    }

    function adicionarAlternativa() {
        var i = $('.ui.alternativas.accordion .title').length + 1;
        $('.ui.alternativas.accordion').append('\
                        <div class="title">\
                            <input id="txtAlternativaIndex" value="'+ i + '" hidden />\
                            <i class="dropdown icon"></i>Alternativa ' + i + '\
                        </div>\
                        <div class="content ui segment">\
                            <div class="field required">\
                                <label for="txtAlternativaEnunciado' + i + '">Enunciado</label>\
                                <textarea id="txtAlternativaEnunciado' + i + '" name="txtAlternativaEnunciado' + i + '" rows="2" placeholder="Enunciado..."></textarea>\
                            </div>\
                            <div class="field">\
                                <div class="ui button">\
                                    Remover\
                                </div>\
                                <div class="ui special popup">\
                                    <div class="header">Tem certeza?</div>\
                                    <div class="content"><p>Essa ação não poderá ser desfeita.</p>\
                                    <div class="ui right aligned remover button tiny">Sim, remover</div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>');
        $('#txtQtdAlternativas').val(i);
        $('.ui.alternativas.accordion .remover.button').off().click(function () {
            removerAlternativa(this);
        });
        $('.ui.alternativas.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
        $('.ui.alternativas.accordion').accordion({animateChildren:false});

    }

    function renomearAlternativas() {
        var list = $('.ui.alternativas.accordion .title');
        var listContent = $('.ui.alternativas.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAlternativaIndex').val();
            list.eq(i).html('<input id="txtAlternativaIndex" value="' + (i + 1) + '" hidden /><i class="dropdown icon"></i>Alternativa ' + (i + 1));
            
            /* RENOMEAR LABELS, INPUTS e TEXTAREAS */
            listContent.eq(i).find('label[for="txtAlternativaEnunciado' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaEnunciado' + j + '"]').attr('name', 'txtAlternativaEnunciado' + (i + 1)).attr('id', 'txtAlternativaEnunciado' + (i + 1));
        }
    }

    function removerAlternativa(button) {
        var i = $('.ui.alternativas.accordion .title').length;
        if (i > 2) {
            var content = $(button).parent().parent().parent().parent();
            var title = $(content).prev();
            title.remove();
            content.remove();
            renomearAlternativas();
            i--;
        } else {
            siac.aviso('É preciso ter, no mínimo, duas alternativas por questão', 'red');
        }
        $('#txtQtdAlternativas').val(i);
    }

    function mostrarCamposPorTipo() {
        var tipo = $('#ddlTipo').val();
        $('.questao.objetiva').hide();
        if (tipo == 1) {
            $('.questao.objetiva').show();
            $('.segment.questao.objetiva').attr('style', '');
        }
        else if (tipo == 2) {
            $('.questao.objetiva').hide();
        }
    }

    function verificar() {
        $('form').removeClass('error');
        $('.ui.error.message .list').html('');
        var validado = false;
        if (!$('#ddlModulo').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um módulo</li>')
        }
        if (!$('#ddlCategoria').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar uma categoria</li>')
        }
        if (!$('#ddlIndicador').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um indicador</li>')
        }

        if (!$('#ddlTipo').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um tipo</li>')
        }

        if (!$('#txtEnunciado').val()) {
            $('.ui.error.message .list').append('<li>É necessário preencher o enunciado</li>')
        }

        if ($('#ddlTipo').val()) {
            var tipo = $('#ddlTipo').val();

            if (tipo == 1) {
                var qteAlternativas = $('#txtQtdAlternativas').val();
                var ok = true;
                for (var i = 0; i < qteAlternativas; i++) {
                    if ($('#txtAlternativaEnunciado' + (i + 1)).val() == '') {
                        ok = false;
                    }
                }
                if (ok) {
                   validado = true;
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher os enunciados de todas as alternativas</li>')
                }
            }
            else if (tipo == 2) {
                if ($('#txtEnunciado').val()) {
                    validado = true;
                }
            }
        }

        if (validado) adicionarQuestao();
        else $('form').addClass('error');
    }

    function adicionarQuestao() {
        _qteQuestoes++;
        //OBTENDO DADOS DA QUESTÃO
        var moduloCod = $('#ddlModulo :selected').val();
        var modulo = $('#ddlModulo :selected').text();
        var categoriaCod = $('#ddlCategoria').val();
        var categoria = $('#ddlCategoria :selected').text();
        var indicadorCod = $('#ddlIndicador').val();
        var indicador = $('#ddlIndicador :selected').text();
        var tipo = $('#ddlTipo').val();
        var enunciado = $('#txtEnunciado').val();
        var observacao = $('#txtObservacao').val();
        var TEMPLATE_QUESTAO_HTML ='<div class="title">'+
                                        '<i class="dropdown icon"></i>'+
                                        modulo+
                                    '</div>'+
                                    '<div class="content" data-modulo="' + moduloCod + '">' +
                                        '<div class="accordion">'+
                                            '<div class="title">'+
                                                '<i class="dropdown icon"></i>'+
                                                categoria + 
                                            '</div>'+
                                            '<div class="content" data-categoria="' + categoriaCod + '">' +
                                                '<div class="accordion">'+
                                                    '<div class="title">'+
                                                        '<i class="dropdown icon"></i>'+
                                                         indicador +
                                                    '</div>'+
                                                    '<div class="content" data-indicador="' + indicadorCod + '">' +
                                                        '<div class="accordion">'+
                                                            '<div class="title">'+
                                                                '<i class="dropdown icon"></i>'+
                                                                    'Questão '+_qteQuestoes+
                                                            '</div>'+
                                                            '<div class="content">'+
                                                                '<div class="ui segment">'+
                                                                    '<h3 class="ui dividing header" data-content="' + observacao + '">'+enunciado+'</h3>'+
                                                                    //<div class="ui very relaxed list">\
                                                                    //    <div class="item">\
                                                                    //        <b>1)</b> ALternativa\
                                                                    //    </div>\
                                                                    //</div>\
                                                                '</div>'+
                                                            '</div>'+
                                                        '</div>'+
                                                    '</div>'+
                                                '</div>'+
                                            '</div>'+
                                        '</div>' +
                                    '</div>';

        //Obtendo possíveis accordions questões por módulos
        var $questoesModulo = $('[data-modulo=' + moduloCod + ']');
        var $questoesCategoria = $('[data-categoria=' + categoriaCod + ']');
        var $questoesIndicador = $('[data-indicador=' + indicadorCod + ']');

        if ($questoesModulo.length > 0) {
            if ($questoesModulo.find('[data-categoria=' + categoriaCod + ']').length > 0) {
                //if ($questoesCategoria.length > 0) {
                if ($questoesModulo.find('[data-categoria=' + categoriaCod + ']').find('[data-indicador=' + indicadorCod + ']').length > 0) {
                //if ($questoesIndicador.length > 0) {
                    $questao = $(TEMPLATE_QUESTAO_HTML).find('[data-indicador=' + indicadorCod + ']').find('.accordion').html();
                    $questoesModulo
                        .find('[data-categoria=' + categoriaCod + ']')
                        .find('[data-indicador=' + indicadorCod + ']')
                        .children('.accordion').append($questao);
                } else {
                    $questao = $(TEMPLATE_QUESTAO_HTML).find('[data-categoria=' + categoriaCod + ']').find('.accordion').html();
                    $questoesModulo
                        .find('[data-categoria=' + categoriaCod + ']')
                        .children('.accordion').append($questao);
                }
            }
            else {
                $questao = $(TEMPLATE_QUESTAO_HTML).find('.accordion').html();
                $questoesModulo
                    .children('.accordion').append($questao);
            }
        }
        else {
            $('.tab.questoes .fluid.styled.accordion').append(TEMPLATE_QUESTAO_HTML);
        }
        $('h3').popup();
        siac.aviso('Questão adicionada com sucesso!', 'green');
        console.log(moduloCod, categoriaCod, indicadorCod);
    }

    function marcarCorreta(chk) {
        $chk = $(chk);
        $lstCheckboxes = $('.ui.alternativas.accordion .content input[type="checkbox"]');
        $('.ui.alternativas.accordion .title .label').attr('style', 'display:none');
        for (var i = 0; i < $lstCheckboxes.length; i++) {
            if ($chk.is(':checked')) {
                if ($chk.attr('id') == $lstCheckboxes.eq(i).attr('id')) {
                    $('.ui.alternativas.accordion .title.active .label').removeAttr('style');
                    continue;
                }
                $lstCheckboxes.eq(i).attr({ 'readonly': 'readonly', 'disabled': 'disabled' });
                $lstCheckboxes.eq(i).parent().addClass('disabled');
            }
            else {
                $lstCheckboxes.eq(i).removeAttr('readonly disabled');
                $lstCheckboxes.eq(i).parent().removeClass('disabled');
            }
        }
    }

    return {
        iniciar: iniciar
    }
})();
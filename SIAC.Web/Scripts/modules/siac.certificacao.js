﻿siac.Certificacao = siac.Certificacao || {};

siac.Certificacao.Gerar = (function () {

	function iniciar() {
		$('.ui.dropdown').dropdown();
		$('.ui.modal').modal();
		$('.ui.termo.modal').modal('show');
		$('.cancelar.button').popup({ on: 'click' });

		$('.ui.confirmar.modal').modal({
			onApprove: function () {
				$('form').addClass('loading').submit();
			}
		});

		$('.prosseguir.button').click(function () {
			prosseguir()
		});

		$('#ddlDisciplina').change(function () {
			listarTemas(this.value);
		})

		$('#ddlTipo').change(function () {
			mostrarOpcoesPorTipo(this.value);
		})


	}

	function prosseguir() {
		var errorList = $('form .error.message .list');

		errorList.html('');
		$('form').removeClass('error');

		var valido = true;

		if (!$('#ddlDisciplina').val()) {
			errorList.append('<li>Selecione uma disciplina</li>');
			valido = false;
		}

		if (!$('#ddlTemas').val()) {
			errorList.append('<li>Selecione ao menos um tema</li>');
			valido = false;
		}

		if (!$('#ddlTipo').val()) {
			errorList.append('<li>Selecione o tipo das questões da sua avaliação acadêmica</li>');
			valido = false;
		}

		if (($('#ddlTipo').val() == 1 || $('#ddlTipo').val() == 3) && !$('#txtQteObjetiva').val()) {
			errorList.append('<li>Indique a quantidade das questões objetivas</li>');
			valido = false;
		}
		if (($('#ddlTipo').val() == 2 || $('#ddlTipo').val() == 3) && !$('#txtQteDiscursiva').val()) {
			errorList.append('<li>Indique a quantidade das questões discursivas</li>');
			valido = false;
		}

		if (!$('#ddlDificuldade').val()) {
			errorList.append('<li>Selecione a dificuldade das questões</li>');
			valido = false;
		}


		if (valido) {
			confirmar();
		}
		else {
			$('form').addClass('error');
			$('html, body').animate({
				scrollTop: $('form .error.message').offset().top
			}, 1000);
		}
	}

	function confirmar() {
		$modal = $('.ui.confirmar.modal');
		$ddlDisciplina = $('#ddlDisciplina :selected');
		$ddlTipo = $('#ddlTipo');
		$table = $modal.find('tbody').html('');

		$tr = $('<tr></tr>');
		$tdDisciplina = $('<td></td>').html('<b>' + $ddlDisciplina.text() + '</b>');
		$tdTemas = $('<td class="ui labels"></td>');

		$ddlTemas = $('#ddlTemas :selected');
		for (var i = 0; i < $ddlTemas.length; i++) {
			$tdTemas.append($('<div class="ui tag label"></div>').text($ddlTemas.eq(i).text()));
		}
		$tdQteQuestoes = $('<td class="ui labels"></td>');
		if ($ddlTipo.val() == 1 || $ddlTipo.val() == 3) {
			$tdQteQuestoes.append($('<div class="ui label"></div>').html('Objetiva<div class="detail">' + $('#txtQteObjetiva').val() + '</div>'));
		}
		if ($ddlTipo.val() == 2 || $ddlTipo.val() == 3) {
			$tdQteQuestoes.append($('<div class="ui label"></div>').html('Discursiva<div class="detail">' + $('#txtQteDiscursiva').val() + '</div>'));
		}
		$tdDificuldade = $('<td></td>').text($('#ddlDificuldade :selected').text());

		$table.append($tr.append($tdDisciplina).append($tdTemas).append($tdQteQuestoes).append($tdDificuldade));

		$modal.modal('show');
	}

	function listarTemas(codDisciplina) {
		var disciplinas = $('#ddlDisciplinas');
		var ddlTemas = $('#ddlTemas');

		ddlTemas.parent().addClass('loading');
		$.ajax({
			type: 'POST',
			url: '/Tema/RecuperarTemasPorCodDisciplinaTemQuestao',
			data: { 'codDisciplina': codDisciplina },
			success: function (data) {
				ddlTemas.html('');
				ddlTemas.val(ddlTemas.prop('defaultSelected'));
				$.each(data, function (id, item) {
					ddlTemas.append('<option value="' + item.CodTema + '">' + item.Descricao + '</option>');
				});

				ddlTemas.parent().removeClass('loading');
			},
			error: function () {
				siac.mensagem("Erro no carregamento de Temas", "Erro");
				ddlTemas.parent().removeClass('loading');
			}
		});
	}

	function mostrarOpcoesPorTipo(tipoAvaliacao) {
		var txtQteObjetiva = $('#txtQteObjetiva');
		var txtQteDiscursiva = $('#txtQteDiscursiva');

		if (tipoAvaliacao == 1) {
			txtQteObjetiva.prop('disabled', false);
			txtQteDiscursiva.prop('disabled', true);
		}
		else if (tipoAvaliacao == 2) {
			txtQteObjetiva.prop('disabled', true);
			txtQteDiscursiva.prop('disabled', false);
		}
		else {
			txtQteObjetiva.prop('disabled', false);
			txtQteDiscursiva.prop('disabled', false);
		}
	}

	return {
		iniciar: iniciar
	}
	
})();
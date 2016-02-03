﻿siac.Utilitario = siac.Utilitario || (function () {
    function formatarData(dataBrasil) {
        if (dataBrasil.indexOf('/') > 0) {
            var hora = '',
                data = '';
            if (dataBrasil.indexOf(' ') > 0) {
                var i = dataBrasil.indexOf(' ');
                hora = dataBrasil.substring(i + 1).trim();
                data = dataBrasil.substring(0, i + 1).trim();
            }
            else if (dataBrasil.indexOf('T') > 0) {
                var i = dataBrasil.indexOf('T');
                hora = dataBrasil.substring(i + 1).trim();
                data = dataBrasil.substring(0, i + 1).trim();
            }
            else {
                data = dataBrasil.trim();
            }
            var temp = data.split('/');
            if (temp) {
                if (temp[0].length == 4) {
                    return (temp[0] + '-' + temp[1] + '-' + temp[2] + 'T' + hora).trim();
                }
                else {
                    return (temp[2] + '-' + temp[1] + '-' + temp[0] + 'T' + hora).trim();
                }
            }
        }
        return dataBrasil;
    }

	// return 1 = a is bigger than b, 0 = a and b are same, -1 = a is smaller than b
    function compararData(strDateA, strDateB) {
		var timeDateA = Date.parse(formatarData(strDateA));
		var timeDateB = Date.parse(formatarData(strDateB));
		if (timeDateA && timeDateB) {
		    if (timeDateA > timeDateB) {
		        return 1;
		    }
		    else if (timeDateA == timeDateB) {
		        return 0;
		    }
		}
		return -1;
	}

	// return true or false
	function dataEFuturo(strData) {
		timeDataAgora = new Date().getTime();
		timeData = Date.parse(formatarData(strData));
		console.log(formatarData(strData), timeData, timeDataAgora)
		if (timeData && timeDataAgora) {
		    return (timeData > timeDataAgora);
		}
		return false;
	}

	function validarCPF(strCPF) {
		var Soma;
		var Resto;
		Soma = 0;
		if (strCPF == "00000000000") return false;
		for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
		Resto = (Soma * 10) % 11; if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(strCPF.substring(9, 10))) return false;
		Soma = 0;
		for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
		Resto = (Soma * 10) % 11; if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(strCPF.substring(10, 11))) return false;
		return true;
	}

	String.prototype.encurtarTextoEm = function (length) {
		var text = '';
		var str = this;
		if (str.length > length) {
			text = str.substring(0, length);
			var afterText = str.substring(length);
			if (afterText.indexOf(' ') > -1) {
				afterText = afterText.substring(0, afterText.indexOf(' '));
				afterText = afterText + '...';
			}
			text = text + afterText;
		}
		else {
			text = str;
		}

		return text;
	}

	String.prototype.quebrarLinhaEm = function (indiceMaximo) {
		texto = this;
		texto = texto.trim();
		if (texto.length > indiceMaximo) {
			var tempMensagem = texto;
			var qteLinha = Math.ceil(texto.length / indiceMaximo);
			mensagem = '';
			for (var i = 0; i <= qteLinha; i++) {
				tempMensagem = tempMensagem.trim();
				if (tempMensagem.length > indiceMaximo) {
					var indice = tempMensagem.substr(0, indiceMaximo).lastIndexOf(' ');
					if (indice == -1) {
						indice = indiceMaximo;
					}
					mensagem += tempMensagem.substr(0, indice) + '<br/>';
					tempMensagem = tempMensagem.substring(indice, tempMensagem.length);
				}
				else {
					mensagem += tempMensagem;
					break;
				}
			}

			return mensagem;
		}
		return texto;
	}

	String.prototype.substituirTodos = function (valorVelho, valorNovo) {
		var _texto = this;
		while (_texto.indexOf(valorVelho) > -1) {
			_texto = _texto.replace(valorVelho, valorNovo);
		};
		return _texto;
	}

	String.prototype.minutosParaStringTempo = function () {
		var minutos = this;
		if (minutos > 59) {
			var strHoras = Math.floor(minutos / 60);
			var strMinutos = (minutos % 60);
			return ('0' + strHoras).slice(-2) + 'h' + ('0' + strMinutos).slice(-2) + 'min';
		}
		else {
			return '00h' + ('0' + minutos).slice(-2) + 'min';
		}
	}

	Number.prototype.inteiroParaLetraMinuscula = function () {
		var numero = this;
		var letras = " abcdefghijklmnopqrstuvxwyz";
		if (numero < letras.length) {
			return letras[numero];
		}
		else {
			return numero;
		}
	}

	return {
		compararData: compararData,
		dataEFuturo: dataEFuturo,
		validarCPF: validarCPF,
        formatarData: formatarData
	}
})();
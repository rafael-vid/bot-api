var host = "http://localhost:63058/api/";
var rdc = "http://localhost:63058/api/";



if (localStorage.getItem("token") == undefined)
    localStorage.setItem("token","");

function TipoUsuario(tipo,key, unidade) {

    document.cookie = "username=tipo; expires=Thu, 18 Dec 2013 12:00:00 UTC";
    document.cookie = "tipo=" + tipo + "; expires=Thu, 18 Dec 2099 12:00:00 UTC; path=/";

    document.cookie = "username=key; expires=Thu, 18 Dec 2013 12:00:00 UTC";
    document.cookie = "key=" + key + "; expires=Thu, 18 Dec 2099 12:00:00 UTC; path=/";

    document.cookie = "username=unidade; expires=Thu, 18 Dec 2013 12:00:00 UTC";
    document.cookie = "unidade=" + unidade + "; expires=Thu, 18 Dec 2099 12:00:00 UTC; path=/";
}


var closeMsg = function (titulo, mensagem, tipo, tempo) {
    var color = "#C46A69";
    if (tempo === null)
        tempo = 1000;
    if (tipo === "S")
        color = "#C46A69";

    $.smallBox({
        title: titulo,
        content: mensagem,
        color: color,
        iconSmall: "fa fa-cloud",
        timeout: tempo
    });
};

function loadParameters() {
    if (localStorage.getItem("token") !== "" || localStorage.getItem("token") !== null) {
        validaLogin();
    } else {
        window.location.href = "Login.html";
    }
}

function falha(data) {
    Swal.fire({
        position: 'top-end',
        icon: 'error',
        title: 'Ops!!! Ocorreu um problema ... Os seus dados não foram salvos. Entre em contato com o administrador.',
        showConfirmButton: false,
        timer: 3500
    })
    //window.location.href = "Login";
}

function validaLogin() {
    var parametro = {
        token: localStorage.getItem("token"),
        user: localStorage.getItem("user")
    };

    comum.get("Agenda/ValidaToken", parametro, function (data) {
        if (data.Msg !== "Ok") {
            window.location.href = "Login.html";
        }
    });
}

function formatarValorSaida(valor) {
    tam = valor.length;
    if (tam <= 2) {
        return valor;
    }
    if ((tam > 2) && (tam <= 6)) {
        return valor.replace(".", ",");
    }
    if ((tam > 6) && (tam <= 9)) {
        return valor.substr(0, tam - 6) + '.' + valor.substr(tam - 6, 3) + ',' + valor.substr(tam - 2, tam);
    }
    if ((tam > 9) && (tam <= 12)) {
        return valor.substr(0, tam - 9) + '.' + valor.substr(tam - 9, 3) + '.' + valor.substr(tam - 6, 3) + ',' + valor.substr(tam - 2, tam);
    }
}

function formatarValorEntrada(valor) {
    if (valor === null)
        valor = 0;

    if (valor !== 0)
        return parseFloat(valor.replace(",", ""))
    else
        return valor

}

function getMoney(el) {
    var money = el.replace(',', '.');
    return parseFloat(money) * 100;
}

function guidGenarator() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}


var comum = {
    
    get: function (metodo, objtJson, funcao) {

        jQuery.ajax({
            type: 'GET',
            url: host + metodo,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                'Authorization': 'Basic ' + localStorage.getItem("token")
            },
            data: objtJson,
            async: false,
            success: function (data) {
                funcao(data);
            },
            error: function () {
                falha(data);
            }

        });

    }, getAsync: function (metodo, objtJson, funcao) {

        jQuery.ajax({
            type: 'GET',
            url: host + metodo,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                'Authorization': 'Basic ' + localStorage.getItem("token")
            },
            data: objtJson,
            async: true,
            success: function (data) {
                funcao(data);
            },
            error: function (data) {
                falha(data);
            }

        });

    },
    post: function (metodo, objtJson, funcao) {
        
        jQuery.ajax({
            type: 'POST',
            url: host + metodo,
            headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json; charset=utf-8',
                    'Authorization': 'Bearer ' + localStorage.getItem("token")
            },
            data: JSON.stringify(objtJson),
            async: false,
            success: function (data) {
                funcao(data);
            },
            error: function (data) {
                falha(data);
            }

        });
    }, postAsync: function (metodo, objtJson, funcao) {

        jQuery.ajax({
            type: 'POST',
            url: host + metodo,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                'Authorization': 'Bearer ' + localStorage.getItem("token")
            },
            data: JSON.stringify(objtJson),
            async: true,
            success: function (data) {
                funcao(data);
            },
            error: function (data) {
                falha(data);
            }

        });
    },
    queryString: function (name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    msg: function (titulo, msg, tipo, tempo) {

        if (tempo === null)
            tempo = 2000;

        var color = "#C46A69";
        var icone = "fa fa-warning shake animated";

        if (tipo === "S") {
            color = "#739E73";
            icone = "fa fa-check";
        }
    }

}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
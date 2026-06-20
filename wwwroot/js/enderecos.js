(function () {
    function somenteDigitos(valor) {
        return valor.replace(/\D/g, "");
    }

    function formatarCep(cep) {
        return cep.replace(/^(\d{5})(\d{3})$/, "$1-$2");
    }

    function obterCampos(formulario) {
        return {
            logradouro: formulario.querySelector('[data-viacep-field="logradouro"]'),
            bairro: formulario.querySelector('[data-viacep-field="bairro"]'),
            localidade: formulario.querySelector('[data-viacep-field="localidade"]'),
            uf: formulario.querySelector('[data-viacep-field="uf"]'),
            ibge: formulario.querySelector('[data-viacep-field="ibge"]')
        };
    }

    function definirFeedback(formulario, mensagem, isErro) {
        var feedback = formulario.querySelector("[data-cep-feedback]");

        if (!feedback) {
            return;
        }

        feedback.textContent = mensagem;
        feedback.classList.toggle("text-danger", isErro);
        feedback.classList.toggle("text-muted", !isErro);
    }

    function limparCampos(formulario) {
        var campos = obterCampos(formulario);

        Object.keys(campos).forEach(function (chave) {
            if (campos[chave]) {
                campos[chave].value = "";
            }
        });
    }

    function preencherCarregando(formulario) {
        var campos = obterCampos(formulario);

        Object.keys(campos).forEach(function (chave) {
            if (campos[chave]) {
                campos[chave].value = "...";
            }
        });
    }

    function preencherCampos(formulario, dados) {
        var campos = obterCampos(formulario);

        if (campos.logradouro) {
            campos.logradouro.value = dados.logradouro || "";
        }

        if (campos.bairro) {
            campos.bairro.value = dados.bairro || "";
        }

        if (campos.localidade) {
            campos.localidade.value = dados.localidade || "";
        }

        if (campos.uf) {
            campos.uf.value = dados.uf || "";
        }

        if (campos.ibge) {
            campos.ibge.value = dados.ibge || "";
        }
    }

    async function pesquisarCep(input) {
        var formulario = input.closest("form");
        var cep = somenteDigitos(input.value);

        if (!formulario || cep.length === 0) {
            return;
        }

        if (!/^\d{8}$/.test(cep)) {
            limparCampos(formulario);
            definirFeedback(formulario, "Formato de CEP inválido.", true);
            return;
        }

        input.value = formatarCep(cep);
        preencherCarregando(formulario);
        definirFeedback(formulario, "Buscando CEP no ViaCEP...", false);

        try {
            var resposta = await fetch("https://viacep.com.br/ws/" + cep + "/json/");
            var dados = await resposta.json();

            if (!resposta.ok || dados.erro) {
                limparCampos(formulario);
                definirFeedback(formulario, "CEP não encontrado.", true);
                return;
            }

            preencherCampos(formulario, dados);
            definirFeedback(formulario, "Endereço preenchido pelo ViaCEP.", false);
        } catch {
            limparCampos(formulario);
            definirFeedback(formulario, "Não foi possível consultar o ViaCEP agora.", true);
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll("[data-cep-input]").forEach(function (input) {
            input.addEventListener("blur", function () {
                pesquisarCep(input);
            });
        });
    });
})();

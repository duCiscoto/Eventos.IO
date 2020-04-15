Funcionalidade: Adicionar novo Evento
	Um Organizador fará seu login pelo site e
	seguirá para a área administrativa onde
	retistrará um novo evento

@TesteAutomatizadoCadastroNovoEvento
Cenário: Registro de Novo Evento
	Dado que o Organizador está no site
	E Realiza o Login
	E Navega até a área administrativa
	E Clica em novo evento
	E Preenche o formulário com os valores
		| Campo                               | Valor                                      |
		| nome                                | DevXperience                               |
		| descricaoCurta                      | Evento de tecnologia                       |
		| descricaoLonga                      | Descrição longa deste evento de tecnologia |
		| categoriaId                         | b43d49a6-aead-4c9c-a8ba-5504e2bfaf5e       |
		| //"[@id="dataInicio"]/div/div/input | 20/10/2019                                 |
		| //"[@id="dataFim"]/div/div/input    | 22/10/2019                                 |
		| gratuito                            | false                                      |
		| valor                               | 1.234,56                                   |
		| online                              | false                                      |
		| nomeEmpresa                         | DevX                                       |
		| logradouro                          | Av. Rio Branco                             |
		| numero                              | 1000                                       |
		| complemento                         | subsolo                                    |
		| bairro                              | Centro                                     |
		| cep                                 | 36030000                                   |
		| cidade                              | Juiz de Fora                               |
		| estado                              | MG                                         |
	Quando clicar no botão adicionar
	Então O evento é registrado e o usuário é redirecionado para a lista de eventos
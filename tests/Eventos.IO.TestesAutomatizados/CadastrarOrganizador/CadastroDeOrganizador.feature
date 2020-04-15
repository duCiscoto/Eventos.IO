Funcionalidade: Cadastro de Organizador
	Um organizador fará seu cadastro pelo site para
	poder gerenciar seus próprios eventos.
	Ao terminar o cadastro,
	receberá uma notificação de sucesso ou de falha.

@TeteAutomatizadoCadastroDeOrganizadorComSucesso
Cenário: Cadastro de Organizador com Sucesso
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | Teste@123       |
		| senhaConfirmacao | TEste@!23       |
	Quando clicar no botão registrar
	Então será registrado e redirecionado com sucesso

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com CPF já utilizado
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor            |
		| nome             | Eduardo          |
		| cpf              | 12345678910      |
		| email            | teste2@teste.com |
		| senha            | Teste@123        |
		| senhaConfirmacao | Teste@!23        |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro de CPF já cadastrado

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Email já utilizado
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678911     |
		| email            | teste@teste.com |
		| senha            | Teste@123       |
		| senhaConfirmacao | Teste@!23       |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro de Email já cadastrado

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Senha sem número
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | Teste@          |
		| senhaConfirmacao | Teste@          |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro que a senha requer número

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Senha sem Maiúscula
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | teste@123       |
		| senhaConfirmacao | teste@123       |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro que a senha requer letra maiúscula

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Senha sem Minúscula
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | TESTE@123       |
		| senhaConfirmacao | TESTE@123       |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro que a senha requer letra minúscula

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Senha sem caracter especial
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | Teste123        |
		| senhaConfirmacao | Teste123        |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro que a senha requer caracter especial

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Senha menor que o tamanhoo permitido
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | Te123           |
		| senhaConfirmacao | Te123           |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro que a senha é menor que o tamanho permitido

@TesteAutomatizadoCadastroOrganizadorFalha
Cenário: Cadastro de Organizador com Senhas diferentes
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor           |
		| nome             | Eduardo         |
		| cpf              | 12345678910     |
		| email            | teste@teste.com |
		| senha            | Teste@123       |
		| senhaConfirmacao | Testa@123       |
	Quando clicar no botão registrar
	Então recebe uma mensagem de erro que a senhas diferentes
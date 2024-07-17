#Desafio Backend

## Vis�o Geral
O projeto � um sitema de gest�o de aluguel de motocicletas `Motorcycle` e entregadores desenvolvido utilizando .NET 8.
O sistema permite que um administrador `Admin` cadastrem motos e modifiquem motos para aluguel.
Tamb�m � poss�vel que entregadores `Renter` se registrem para alugarem motocicletas.

## Tecnologias
- .NET 8
- Entity Framework Core
- Postgres
- Docker e Docker Compose..
- RabbitMQ
- MinIO
- smtp4dev

## Servi�os
- `Authentication.API` - Servi�o de autentica��o
- `Core.API` - Servi�o de gest�o de motocicletas e alugueis
- `Worker.API` - Servi�o de processamento de eventos
## Outros Servi�os Utilizados
� poss�vel alterar a variavel de ambiente `ASPNETCORE_ENVIRONMENT` no `docker-compose.yml` para `Development` ou para `Cloud` para que os servi�os possam utilizar Docker ou servi�os na nuvem.
- Rodando em Docker: 
	- `pgAdmin` - Interface gr�fica para o banco de dados
	- `Postgres` - Banco de dados
	- `RabbitMQ` - Mensageria
	- `smtp4dev` - Servi�o de email
- Rodando na Cloud:
	- `MinIO` - Storage (�nico servi�o na nuvem tanto utilizando `Development` quanto `Cloud`)
	- `Postgres` - Banco de dados
	- `RabbitMQ` - Mensageria


## Entidades
### Authentication.API
- `User` - Usu�rio base do sistema, podendo ser um `Admin` ou `Renter`
### Core.API
- `Motorcycle` - Entidade que representa uma motocicleta
- `Rent` - Entidade que representa um aluguel
- `Renter` - Entidade que representa um entregador, uma pessoa que aluga uma motocicleta
### Worker.API
- `EventData` - Entidade que representa um evento

## Casos de Uso
- [x] Eu como usu�rio admin quero cadastrar uma nova moto.
	- [x] Os dados obrigat�rios da moto s�o Identificador, Ano, Modelo e Placa
	- [x] A placa � um dado �nico e n�o pode se repetir.
	- [x] Quando a moto for cadastrada a aplica��o dever� gerar um evento de moto cadastrada
		- [x] A notifica��o dever� ser publicada por mensageria.
		- [x] Criar um consumidor para notificar quando o ano da moto for "2024"
		- [x] Assim que a mensagem for recebida, dever� ser armazenada no banco de dados para consulta futura.

- [x] Eu como usu�rio admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- [x] Eu como usu�rio admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- [x] Eu como usu�rio admin quero remover uma moto que foi cadastrado incorretamente, desde que n�o tenha registro de loca��es.
- [x] Eu como usu�rio entregador quero me cadastrar na plataforma para alugar motos.
	- [x] Os dados do entregador s�o( identificador, nome, cnpj, data de nascimento, n�mero da CNHh, tipo da CNH, imagemCNH)
	- [x] Os tipos de cnh v�lidos s�o A, B ou ambas A+B.
	- [x] O cnpj � �nico e n�o pode se repetir.
	- [x] O n�mero da CNH � �nico e n�o pode se repetir.

- [x] Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
	- [x] O formato do arquivo deve ser png ou bmp.
	- [x] A foto n�o poder� ser armazenada no banco de dados, voc� pode utilizar um servi�o de storage( disco local, amazon s3, minIO ou outros).
	- [x] Eu como entregador quero alugar uma moto por um per�odo.
		- Os planos dispon�veis para loca��o s�o:
		- [x] 7 dias com um custo de R$30,00 por dia
		- [x] 15 dias com um custo de R$28,00 por dia
		- [x] 30 dias com um custo de R$22,00 por dia
		- [x] 45 dias com um custo de R$20,00 por dia
		- [x] 50 dias com um custo de R$18,00 por dia
	- [x] A loca��o obrigat�riamente tem que ter uma data de inicio e uma data de t�rmino e outra data de previs�o de t�rmino. 
	- [x] O inicio da loca��o obrigat�riamente � o primeiro dia ap�s a data de cria��o. 
	- [x] Somente entregadores habilitados na categoria A podem efetuar uma loca��o 
- [x] Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da loca��o.
	- [x] Quando a data informada for inferior a data prevista do t�rmino, ser� cobrado o valor das di�rias e uma multa adicional
		- [x] Para plano de 7 dias o valor da multa � de 20% sobre o valor das di�rias n�o efetivadas.
		- [x] Para plano de 15 dias o valor da multa � de 40% sobre o valor das di�rias n�o efetivadas.
	- [x] Quando a data informada for superior a data prevista do t�rmino, ser� cobrado um valor adicional de R$50,00 por di�ria adicional.

## CQRS
O projeto foi desenvolvido utilizando o padr�o [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs), onde as opera��es de leitura e escrita s�o separadas.
## Pr�-Requisitos para execu��o
- [GIT](https://git-scm.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)

### Verificar se os pr�-requisitos est�o instalados
Verificar o git est� instalado
```bash
git --version
```
Verificar se a vers�o corresponde a vers�o 8.0
```bash
dotnet --version
```
Verificar se o docker est� instalado
```bash
docker --version
```
### Clonar reposit�rio
```bash
git clone https://github.com/bsbrixius/rentalApp.git
```

### Rodar docker-compose
- Observa��o: para os servi�os `Authentication.API`, `Core.API` e `Worker.API` 
no docker-compose.yml � poss�vel alterar a variavel de ambiente `ASPNETCORE_ENVIRONMENT` no `docker-compose.yml` para `Development` ou para `Cloud` para que os servi�os possam utilizar Docker ou servi�os na nuvem.

```bash
docker-compose up
```
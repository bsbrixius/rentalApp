#Desafio Backend

## Visão Geral
O projeto é um sitema de gestão de aluguel de motocicletas `Motorcycle` e entregadores desenvolvido utilizando .NET 8.
O sistema permite que um administrador `Admin` cadastrem motos e modifiquem motos para aluguel.
Também é possível que entregadores `Renter` se registrem para alugarem motocicletas.

## Tecnologias
- .NET 8
- Entity Framework Core
- Postgres
- Docker e Docker Compose..
- RabbitMQ
- MinIO
- smtp4dev

## Serviços
- `Authentication.API` - Serviço de autenticação
- `Core.API` - Serviço de gestão de motocicletas e alugueis
- `Worker.API` - Serviço de processamento de eventos
## Outros Serviços Utilizados
É possível alterar a variavel de ambiente `ASPNETCORE_ENVIRONMENT` no `docker-compose.yml` para `Development` ou para `Cloud` para que os serviços possam utilizar Docker ou serviços na nuvem.
- Rodando em Docker: 
	- `pgAdmin` - Interface gráfica para o banco de dados
	- `Postgres` - Banco de dados
	- `RabbitMQ` - Mensageria
	- `smtp4dev` - Serviço de email
- Rodando na Cloud:
	- `MinIO` - Storage (Único serviço na nuvem tanto utilizando `Development` quanto `Cloud`)
	- `Postgres` - Banco de dados
	- `RabbitMQ` - Mensageria


## Entidades
### Authentication.API
- `User` - Usuário base do sistema, podendo ser um `Admin` ou `Renter`
### Core.API
- `Motorcycle` - Entidade que representa uma motocicleta
- `Rent` - Entidade que representa um aluguel
- `Renter` - Entidade que representa um entregador, uma pessoa que aluga uma motocicleta
### Worker.API
- `EventData` - Entidade que representa um evento

## Casos de Uso
- [x] Eu como usuário admin quero cadastrar uma nova moto.
	- [x] Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
	- [x] A placa é um dado único e não pode se repetir.
	- [x] Quando a moto for cadastrada a aplicação deverá gerar um evento de moto cadastrada
		- [x] A notificação deverá ser publicada por mensageria.
		- [x] Criar um consumidor para notificar quando o ano da moto for "2024"
		- [x] Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.

- [x] Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- [x] Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- [x] Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- [x] Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
	- [x] Os dados do entregador são( identificador, nome, cnpj, data de nascimento, número da CNHh, tipo da CNH, imagemCNH)
	- [x] Os tipos de cnh válidos são A, B ou ambas A+B.
	- [x] O cnpj é único e não pode se repetir.
	- [x] O número da CNH é único e não pode se repetir.

- [x] Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
	- [x] O formato do arquivo deve ser png ou bmp.
	- [x] A foto não poderá ser armazenada no banco de dados, você pode utilizar um serviço de storage( disco local, amazon s3, minIO ou outros).
	- [x] Eu como entregador quero alugar uma moto por um período.
		- Os planos disponíveis para locação são:
		- [x] 7 dias com um custo de R$30,00 por dia
		- [x] 15 dias com um custo de R$28,00 por dia
		- [x] 30 dias com um custo de R$22,00 por dia
		- [x] 45 dias com um custo de R$20,00 por dia
		- [x] 50 dias com um custo de R$18,00 por dia
	- [x] A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término. 
	- [x] O inicio da locação obrigatóriamente é o primeiro dia após a data de criação. 
	- [x] Somente entregadores habilitados na categoria A podem efetuar uma locação 
- [x] Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
	- [x] Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
		- [x] Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
		- [x] Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
	- [x] Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.

## CQRS
O projeto foi desenvolvido utilizando o padrão [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs), onde as operações de leitura e escrita são separadas.
## Pré-Requisitos para execução
- [GIT](https://git-scm.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)

### Verificar se os pré-requisitos estão instalados
Verificar o git está instalado
```bash
git --version
```
Verificar se a versão corresponde a versão 8.0
```bash
dotnet --version
```
Verificar se o docker está instalado
```bash
docker --version
```
### Clonar repositório
```bash
git clone https://github.com/bsbrixius/rentalApp.git
```

### Rodar docker-compose
- Observação: para os serviços `Authentication.API`, `Core.API` e `Worker.API` 
no docker-compose.yml é possível alterar a variavel de ambiente `ASPNETCORE_ENVIRONMENT` no `docker-compose.yml` para `Development` ou para `Cloud` para que os serviços possam utilizar Docker ou serviços na nuvem.

```bash
cd rentalApp
docker-compose up
```

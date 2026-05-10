# language: pt
Funcionalidade: Monitoramento de consumo de energia
  Como gestor da EcoCity
  Quero registrar leituras de consumo de energia
  Para que o sistema classifique o status conforme o limite

  Cenário: Consumo dentro do limite gera status Normal
    Dado que envio uma leitura de energia com consumo 500 kWh e limite 1000 kWh
    Quando a requisição POST é enviada para "/api/Energia"
    Então o código de resposta deve ser 201
    E o campo "status" da resposta deve ser "Normal"
    E a resposta deve respeitar o schema "energia.schema.json"

  Cenário: Consumo acima do limite gera status Acima do limite
    Dado que envio uma leitura de energia com consumo 1500 kWh e limite 1000 kWh
    Quando a requisição POST é enviada para "/api/Energia"
    Então o código de resposta deve ser 201
    E o campo "status" da resposta deve ser "Acima do limite"

  Cenário: Payload inválido retorna erro 400
    Dado que envio um payload de energia inválido
    Quando a requisição POST é enviada para "/api/Energia"
    Então o código de resposta deve ser 400

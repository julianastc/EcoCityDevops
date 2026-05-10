# language: pt
Funcionalidade: Coleta de resíduos
  Como gestor de coleta da EcoCity
  Quero registrar volumes de resíduos
  Para que o sistema indique a urgência da coleta

  Cenário: Volume baixo retorna status Normal
    Dado que envio uma coleta no bairro "Centro" com volume 100 kg e limite 500 kg
    Quando a requisição POST é enviada para "/api/Residuos"
    Então o código de resposta deve ser 201
    E o campo "coletaStatus" da resposta deve ser "Normal"
    E a resposta deve respeitar o schema "residuo.schema.json"

  Cenário: Volume excedido retorna status Urgente
    Dado que envio uma coleta no bairro "Vila" com volume 600 kg e limite 500 kg
    Quando a requisição POST é enviada para "/api/Residuos"
    Então o código de resposta deve ser 201
    E o campo "coletaStatus" da resposta deve ser "Urgente"

  Cenário: Listagem de resíduos retorna lista válida
    Quando a requisição GET é enviada para "/api/Residuos"
    Então o código de resposta deve ser 200
    E o corpo da resposta deve ser uma lista JSON

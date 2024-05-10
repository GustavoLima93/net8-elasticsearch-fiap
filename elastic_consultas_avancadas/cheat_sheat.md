# Exemplos

## Exemplo 1

```JSON

PUT /exemplo
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  },
  "mappings": {
    "properties": {
      "nome": { "type": "text" },
      "idade": { "type": "integer" },
      "interesses": { "type": "text" }
    }
  }
}

```

```JSON


POST /exemplo/_doc/1
{
  "nome": "Alice",
  "idade": 30,
  "interesses": ["esqui", "escalada"]
}

POST /exemplo/_doc/2
{
  "nome": "Bob",
  "idade": 35,
  "interesses": ["natação", "ciclismo"]
}

POST /exemplo/_doc/3
{
  "nome": "Carol",
  "idade": 25,
  "interesses": ["corrida", "esqui"]
}

```

```json


GET /exemplo/_search
{
  "query": {
    "bool": {
      "must": {
        "match": { "interesses": "esqui" }
      },
      "filter": {
        "range": { "idade": { "gte": 30 }}
      }
    }
  }
}

```

## Exemplo 2

```json

GET /livros/_search
{
  "query": {
    "match": {
      "titulo": {
        "query": "misterio ilha",
        "operator": "and"
      }
    }
  }
}

```

```json


GET /livros/_search
{
  "query": {
    "bool": {
      "must": [
        { "match": { "titulo": "aventura" }},
        { "match": { "autor": "Julio Verne" }}
      ]
    }
  }
}

```

```json


GET /livros/_search
{
  "query": {
    "bool": {
      "should": [
        { "match": { "descricao": { "query": "ficção científica", "boost": 2 }}},
        { "match": { "descricao": "robótica" }}
      ]
    }
  }
}


```

```json


GET /produtos/_search


{
  "query": {
    "bool": {
      "should": [
        { "match": { "interesses": "natação" }},
        { "match": { "interesses": "esqui" }}
      ],
      "filter": {
        "range": { "idade": { "gte": 30 }}
      },
      "minimum_should_match": 1
    }
  }
}



```

```json


GET /noticias/_search
{
  "query": {
    "bool": {
      "should": [
        { "match": { "conteudo": "eleições" }},
        { "match": { "conteudo": "política" }},
        { "match": { "conteudo": "voto" }}
      ],
      "minimum_should_match": "50%"
    }
  }
}

```

```json

GET /imoveis/_search
{
  "query": {
    "bool": {
      "must": { "match": { "quartos": "3" }},
      "should": [
        { "match": { "localizacao": "Nova York" }},
        { "match": { "localizacao": "Los Angeles" }}
      ],
      "minimum_should_match": 1
    }
  }
}

```

## Exemplo 3

```json


PUT /artigos
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  },
  "mappings": {
    "properties": {
      "titulo": { "type": "text" },
      "conteudo": { "type": "text" }
    }
  }
}


```

```json


POST /artigos/_doc/1
{
  "titulo": "Explorando o Espaço",
  "conteudo": "A última missão à Lua foi incrível."
}

POST /artigos/_doc/2
{
  "titulo": "Tecnologia e Hoje",
  "conteudo": "Explorando novas tecnologias em computação."
}

POST /artigos/_doc/3
{
  "titulo": "Viagem Espacial: O Futuro",
  "conteudo": "Explorando o espaço continua a ser um objetivo importante para a humanidade."
}



```

```json


GET /artigos/_search
{
  "query": {
    "bool": {
      "should": [
        { "match": { "titulo": { "query": "espaço", "boost": 2 }}},
        { "match": { "conteudo": "explorando" }}
      ]
    }
  }
}


  

```

## Exemplo 4

```json


PUT /produtos
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  },
  "mappings": {
    "properties": {
      "nome": { "type": "text" },
      "descricao": { "type": "text" }
    }
  }
}



```

```json


POST /produtos/_doc/1
{
  "nome": "iPhone 12",
  "descricao": "O iPhone 12 é o mais recente smartphone da Apple."
}

POST /produtos/_doc/2
{
  "nome": "iPad Pro",
  "descricao": "O iPad Pro oferece desempenho incrível e uma tela grande."
}

POST /produtos/_doc/3
{
  "nome": "iMac 2021",
  "descricao": "O iMac 2021 redefine o design de computadores desktop."
}



  

```

```json


GET /produtos/_search
{
  "query": {
    "prefix": {
      "nome": {
        "value": "iP"
      }
    }
  }
}


  

```

## Exemplo 5

```json

PUT /biblioteca
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  },
  "mappings": {
    "properties": {
      "titulo": { "type": "text" }
    }
  }
}

```

```json


POST /biblioteca/_doc/1
{
  "titulo": "As Aventuras de Sherlock Holmes"
}

POST /biblioteca/_doc/2
{
  "titulo": "A História de Robin Hood"
}

POST /biblioteca/_doc/3
{
  "titulo": "As Viagens de Gulliver"
}

```

```json

GET /biblioteca/_search
{
  "query": {
    "match_phrase_prefix": {
      "titulo": {
        "query": "As"
      }
    }
  }
}

```

## Exemplo 6

```json

PUT /produtos
{
  "settings": {
    "analysis": {
      "analyzer": {
        "meu_analisador_ngram": {
          "type": "custom",
          "tokenizer": "meu_tokenizer_ngram"
        }
      },
      "tokenizer": {
        "meu_tokenizer_ngram": {
          "type": "ngram",
          "min_gram": 3,
          "max_gram": 3
        }
      }
    }
  },
  "mappings": {
    "properties": {
      "nome": {
        "type": "text",
        "analyzer": "meu_analisador_ngram"
      }
    }
  }
}

```

```json


POST /produtos/_doc/1
{
  "nome": "iPhone"
}

POST /produtos/_doc/2
{
  "nome": "iPad"
}

POST /produtos/_doc/3
{
  "nome": "MacBook"
}


```

```json

GET /produtos/_search
{
  "query": {
    "match": {
      "nome": "Pad"
    }
  }
}
  

```

## Exemplo 7

```json

PUT /loja_online
{
  "settings": {
    "analysis": {
      "analyzer": {
        "ngram_analyzer": {
          "type": "custom",
          "tokenizer": "ngram_tokenizer"
        }
      },
      "tokenizer": {
        "ngram_tokenizer": {
          "type": "ngram",
          "min_gram": 1,
          "max_gram": 2
        }
      }
    }
  },
  "mappings": {
    "properties": {
      "nome_produto": {
        "type": "text",
        "analyzer": "ngram_analyzer"
      }
    }
  }
}



```

```json


POST /loja_online/_doc/1
{
  "nome_produto": "Smartphone Galaxy"
}

POST /loja_online/_doc/2
{
  "nome_produto": "Fone de Ouvido Bluetooth"
}

POST /loja_online/_doc/3
{
  "nome_produto": "Carregador Portátil"
}


  

```

```json



GET /loja_online/_search

{ "query": { "match": { "nome_produto": "Bluetoo" } } }

  

```

## Exemplo 8

```json

PUT /livraria

{
  "settings": {
    "analysis": {
      "analyzer": {
        "edge_ngram_analyzer": {
          "type": "custom",
          "tokenizer": "edge_ngram_tokenizer"
        }
      },
      "tokenizer": {
        "edge_ngram_tokenizer": {
          "type": "edge_ngram",
          "min_gram": 2,
          "max_gram": 5
        }
      }
    }
  },
  "mappings": {
    "properties": {
      "titulo": {
        "type": "text",
        "analyzer": "edge_ngram_analyzer"
      }
    }
  }
}



```

```json

POST /livraria/_doc/1
{
  "titulo": "Guerra e Paz"
}

POST /livraria/_doc/2
{
  "titulo": "Cem Anos de Solidão"
}

POST /livraria/_doc/3
{
  "titulo": "O Grande Gatsby"
}


  

```

```json



GET /livraria/_search

{ "query": { "match": { "titulo": "Gu" } } }

  

```

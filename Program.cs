using NLP;
using NLP.Models;

Console.WriteLine("NLP WORD CONQUERER");

//Training();
Predict();

void Training()
{

    string[] list1 = [
        "a shigelose é uma infecção intestinal causada por bactérias do gênero shigella. a espécie shigella dysenteriae é considerada a mais grave e é capaz de causar diarreia com sangue e mucosas, além de febre, dor abdominal e náuseas. a transmissão pode ocorrer por água ou alimentos contaminados, além do contato direto com fezes infectadas.",
        "a febre paratifóide b é uma infecção bacteriana aguda causada pela bactéria salmonella paratyphi b.",
        "essa categoria inclui outras intoxicações alimentares bacterianas não especificadas, exceto aquelas causadas por salmonella, shigella, campylobacter e clostridium perfringens",
        "doença parasitária causada pelo protozoário giardia lamblia, que afeta principalmente o trato gastrointestinal humano.",
        "doença infecciosa causada pela bactéria mycobacterium tuberculosis, que afeta principalmente os pulmões e pode ser transmitida pelo ar."
    ];

    string[] list2 = [
        "doenças causadas pela exposição a poluentes presentes no solo, como metais pesados e substâncias químicas tóxicas.",
        "a exposição ocupacional a agentes tóxicos em outras indústrias pode ocorrer em diversos setores, como em trabalhadores que lidam com produtos químicos, metais pesados, solventes e outras substâncias tóxicas. essa exposição pode causar problemas neurológicos, respiratórios, dermatológicos e até mesmo câncer.",
        "essa categoria inclui doenças e transtornos relacionados ao trabalho que não se enquadram em outras categorias específicas. alguns exemplos incluem dores musculares e articulares, fadiga, ansiedade e estresse ocupacional.",
        "problema relacionado com o medo ou possibilidade de perder o emprego atual, o que pode levar a ansiedade, estresse e outros problemas emocionais",
        "esta categoria inclui problemas relacionados com a educação e a alfabetização que não se enquadram nas categorias anteriores, como dificuldades de aprendizagem, transtornos de déficit de atenção e hiperatividade, entre outros.",
        "analfabetismo é a incapacidade de ler e escrever, enquanto baixa escolaridade se refere a indivíduos que possuem pouca escolaridade formal. ambas são consideradas fatores de risco para diversas doenças e problemas de saúde, como a falta de acesso à informação e dificuldade em compreender informações importantes sobre saúde."
    ];


    string[] list3 = [
        "não foi encontrado dados precisos sobre a taxa de ocorrência e letalidade específicas para essa doença, mas pode-se considerar os dados gerais da doença diverticular do intestino grosso",
        "a fissura anal aguda é uma lesão na pele que reveste o ânus e pode ser bastante dolorosa. ela ocorre principalmente em adultos jovens e saudáveis e é mais comum em mulheres do que em homens.",
        "o volvo é uma condição rara e grave que ocorre quando uma porção do intestino se torce em torno de sua própria base, interrompendo o fluxo sanguíneo para essa área. isso pode levar à morte do tecido intestinal e à perfuração do intestino, o que pode levar a uma infecção grave.",
        "a fístula anorretal é uma comunicação anormal que se forma entre o canal anal ou reto e a pele ao redor do ânus. a causa mais comum é uma infecção do trato anal ou reto, que pode levar a um abscesso que não foi tratado adequadamente. ",
        "o prolapso anal é uma condição em que a mucosa retal se projeta através do ânus, podendo ser classificado em três tipos: prolapso mucoso, prolapso retal completo e prolapso retal interno.",
        "a proctite por radiação é uma inflamação da mucosa retal que ocorre como resultado da radioterapia pélvica. é comum em pacientes com câncer de próstata, câncer de reto ou outros tipos de câncer pélvico que são tratados com radioterapia."
    ];


    NLP.Classify.Instance()
        .Model("word-conquerer-attention-model.bin")
        .AddCategories("CatA", list1)
        .AddCategories("CatZ", list2)
        .AddCategories("CatK", list3)
        .Train();
}



void Predict()
{
    NLP.Classify classifier = NLP.Classify.Instance().Model("word-conquerer-attention-model.bin", true);
    NLP.Models.Result[] results1 = classifier.Predict("problema relacionado com o medo ou possibilidade de perder o emprego atual tuberculosis, que afeta principalmente", 2);
    NLP.Classify.Print(results1);
    Console.WriteLine();
    NLP.Models.Result[] results2 = classifier.Predict("intoxicações alimentares bacterianas não especificadas, exceto aquelas causadas por salmonella", 2);
    NLP.Classify.Print(results2);
    Console.WriteLine();
    NLP.Models.Result[] results3 = classifier.Predict("através do ânus, podendo ser classificado em três tipos: prolapso mucos tuberculosis, que afeta principalmente", 2);
    NLP.Classify.Print(results3);
    Console.WriteLine();


    /*NLP.Models.Result[] results1 = NLP.Classify.Instance()
        .Model("word-conquerer-attention-model.bin", true)
        .Predict("problema relacionado com o medo ou possibilidade de perder o emprego atual tuberculosis, que afeta principalmente", 2);

    NLP.Classify.Print(results1);
    Console.WriteLine();

    NLP.Models.Result[] results2 = NLP.Classify.Instance()
        .Model("word-conquerer-attention-model.bin", true)
        .Predict("intoxicações alimentares bacterianas não especificadas, exceto aquelas causadas por salmonella", 2);

    NLP.Classify.Print(results2);
    Console.WriteLine();

    NLP.Models.Result[] results3 = NLP.Classify.Instance()
        .Model("word-conquerer-attention-model.bin", true)
        .Predict("através do ânus, podendo ser classificado em três tipos: prolapso mucos tuberculosis, que afeta principalmente", 2);

    NLP.Classify.Print(results3);
    Console.WriteLine();*/

}
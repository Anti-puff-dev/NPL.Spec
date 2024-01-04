using Microsoft.Extensions.Configuration;
using NLP;
using NLP.Models;
using MySQL;
using System.Data;
using MySQL.Service;

Console.WriteLine("NLP WORD CONQUERER");

//Training();
//Predict();
//TrainingFromDb();
//PredictCidClass();
QnA();

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
        .UsePreAttention(false)
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



void TrainingFromDb()
{
    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
    var configuration = builder.Build();

    string string_connection = configuration["DefaultConnectionString"];
    Dictionary<string, List<string>> htable = new Dictionary<string, List<string>>();
    int c_all = 0;
    int c = 0;

    DataSet ds_data = Data.IQuery("SELECT cid_sub_categoria.id, cid_sub_categoria.categoria_id, cid_sub_categoria.descricao, cid_sub_categoria.info, cid_categoria.descricao AS nome FROM cid_sub_categoria LEFT JOIN cid_categoria ON cid_categoria.id=cid_sub_categoria.categoria_id  ORDER BY id ASC", string_connection, new string[] { });

    foreach (DataRow dr in ds_data.Tables[0].Rows)
    {

        if (!htable.ContainsKey(dr["id"].ToString().Substring(0, 1)))
        {
            //Console.WriteLine(dr["id"].ToString().Substring(0, 1));
            htable[dr["id"].ToString().Substring(0, 1)] = new List<string>();
        }
        ((List<string>)htable[dr["id"].ToString().Substring(0, 1)]).Add(dr["info"].ToString());
    }

    Console.Clear();

    c_all = htable.Count;


    NLP.Classify classifier = NLP.Classify.Instance().Model("conquerer-cid10-attention-model.bin");

    foreach (KeyValuePair<string, List<string>> item in htable)
    {
        double p = (100 * (c + 1) / c_all);

        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"Query data  {c + 1}/{c_all} {Math.Ceiling(p)}% ");
        Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 0; i < (int)Math.Ceiling(p / 4); i++)
        {
            Console.Write("#");
        }
        Console.Write("                                                        ");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;

        //Console.WriteLine($"{item.Key.ToString()} {((List<string>)item.Value).Count()}");
        classifier.AddCategories(item.Key.ToString(), ((List<string>)item.Value).ToArray());
        c++;
    }

    classifier.UsePreAttention(true);
    classifier.Train();
}



void PredictCidClass()
{
    NLP.Classify classifier = NLP.Classify.Instance().Model("conquerer-cid10-attention-model.bin", true);
    classifier.Dropout(0.000001);

    //C
    NLP.Models.Result[] results1 = classifier.Predict("o linfoma não-hodgkin difuso é um tipo de câncer que afeta o sistema linfático, responsável pela defesa do organismo. é caracterizado por células malignas que se espalham rapidamente por todo o corpo.", 2);
    NLP.Classify.Print(results1);
    Console.WriteLine();

    //Z
    NLP.Models.Result[] results2 = classifier.Predict("convalescença é o período de recuperação após uma doença ou tratamento médico, e tratamento combinado é quando mais de um tipo de tratamento é utilizado para tratar uma doença.", 2);
    NLP.Classify.Print(results2);
    Console.WriteLine();

    //K
    NLP.Models.Result[] results3 = classifier.Predict("a fístula anorretal é uma comunicação anormal que se forma entre o canal anal ou reto e a pele ao redor do ânus. a causa mais comum é uma infecção do trato anal ou reto, que pode levar a um abscesso que não foi tratado adequadamente.", 2);
    NLP.Classify.Print(results3);
    Console.WriteLine();

    //T
    NLP.Models.Result[] results4 = classifier.Predict("o gás lacrimogêneo é um composto químico irritante utilizado em técnicas de controle de multidões. os sintomas de exposição incluem lacrimejamento, irritação dos olhos, tosse e dificuldade em respirar.", 2);
    NLP.Classify.Print(results4);
    Console.WriteLine();

    //A
    NLP.Models.Result[] results5 = classifier.Predict("essa categoria inclui outras intoxicações alimentares bacterianas não especificadas, exceto aquelas causadas por salmonella, shigella, campylobacter e clostridium perfringen", 2);
    NLP.Classify.Print(results5);
    Console.WriteLine();
}


void QnA()
{
    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
    var configuration = builder.Build();

    string string_connection = configuration["DefaultConnectionString"]!;

    NLP.MySQL.Result[] results = NLP.MySQL.QnA.Instance("cid_sub_categoria", "info").Connection(string_connection).Predict("o que é a malária");

    foreach(NLP.MySQL.Result r in results)
    {
        Console.WriteLine($"{r.phrase}");
        //Console.WriteLine($"{r.confidence}");
    }
}
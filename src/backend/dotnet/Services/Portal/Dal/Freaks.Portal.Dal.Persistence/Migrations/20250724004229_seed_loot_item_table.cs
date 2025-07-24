using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freaks.Portal.Dal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seed_loot_item_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "loot_item",
                schema: "portal",
                columns: new[] { "loot_type", "grade_type", "item_name", "item_description", "synthesis_exp", "icon_uri" },
                values: new object[,]
                {
                    { 1, 3, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 1000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 4, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 2000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 5, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 3000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 6, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 9000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 7, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 6000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 8, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 8000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 9, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 10000, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 1, 10, "Эссенция ярости", "Используется для синтеза трофейного снаряжения мифических противников.", 13500, "http://localhost:11000/freaks/loot_items/icons/icon_item_4687.png" },
                    { 2, 13, "Трофейная эссенция стихий", "Используется в качестве вспомогательного материала при синтезе эфенского снаряжения такого же либо более низкого качества.", 100, "http://localhost:11000/freaks/loot_items/icons/icon_item_3094.png" },
                    { 3, 2, "Призма лунного акхиума", "Магическая субстанция, необходимая для изготовления доспехов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0367.png" },
                    { 3, 2, "Призма солнечного акхиума", "Магическая субстанция, необходимая для изготовления оружия.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0366.png" },
                    { 3, 2, "Призма звездного акхиума", "Магическая субстанция, необходимая для изготовления украшений.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0368.png" },
                    { 3, 2, "Эссенция лунного акхиума", "Магическая субстанция, необходимая для изготовления доспехов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0370.png" },
                    { 3, 2, "Эссенция солнечного акхиума", "Магическая субстанция, необходимая для изготовления оружия.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0369.png" },
                    { 3, 2, "Эссенция звездного акхиума", "Магическая субстанция, необходимая для изготовления украшений.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0371.png" },
                    { 4, 6, "Рави`мар, Драконья ярость", "Этот клинок изготовлен из таинственного сплава, секрет которого был утрачен после ухода нуонов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4541.png" },
                    { 4, 6, "Дра'кордис, Сердце дракона", "Эта булава изготовлена из таинственного сплава, секрет которого был утрачен после ухода нуонов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4540.png" },
                    { 4, 6, "Ташш, змеиное жало", "Изогнутое лезвие этого фальшиона напоминает жало змеи – излюбленной боевой формы нагшасов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_blade_1h_0103.png" },
                    { 4, 6, "Нирах, искушающий", "Взяв в руки этот жезл с навершием в виде золотой змеи, вы услышите едва различимый шепот. Но берегитесь: те, кто вслушивается слишком старательно, рано или поздно лишаются рассудка.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_staff_1h_0105.png" },
                    { 4, 6, "Мор’гур, Смерть драконов", "Этот меч изготовлен из таинственного сплава, секрет которого был утрачен после ухода нуонов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4544.png" },
                    { 4, 6, "Вул’данор, Клеймитель драконов", "Этот посох изготовлен из таинственного сплава, секрет которого был утрачен после ухода нуонов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4545.png" },
                    { 4, 6, "Ишхар, грань измерений", "Рукоять этого меча обтянута змеиной кожей, а лезвие напоено яростью пленников, заключенных в стенах Нагашара.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_sword_2h_0087.png" },
                    { 4, 6, "Гирра, пробивающий брешь", "Перед мощью этого боевого топора не устоят даже анадиевые стены Нагашара.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_axe_2h_0070.png" },
                    { 4, 6, "Иг'нис, Пламя возмездия", "Этот лук изготовлен из прочнейшей кости древнего дракона.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4543.png" },
                    { 4, 6, "Дра'орис, Дыхание дракона", "Эта винтовка изготовлена из таинственного сплава, секрет которого был утрачен после ухода нуонов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_shotgun_0053.png" },
                    { 4, 6, "Джераб, слуга смерти", "Каждый выстрел, совершенный из этой винтовки, - жертва, принесенная богине смерти, матери нагшасов.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_shotgun_0054.png" },
                    { 4, 6, "Нерхал, бронзовая чешуя", "Этот прочный щит окован закаленной медью, украшенной узором в виде змеиной чешуи.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_shield_0055.png" },
                    { 4, 6, "Аст'аджал, Длань судьбы", "Эта алебарда принадлежала инистерскому рыцарю Триссену, который пообещал одолеть кракена во славу своей прекрасной леди. Чудовище проглотило героя целиком. Теперь это оружие наполнено темной силой морских глубин.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_spear_2h_0041.png" },
                    { 4, 6, "Ро'кана, Безумие морей", "Этот посох принадлежал безумному мудрецу Кассандросу из Ост-Терры, который однажды пустился по волнам в утлой лодчонке, чтобы найти и освободить Дауту. Кракен нашел его первым. Теперь это оружие наполнено темной силой морских глубин.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_staff_1h_0058.png" },
                    { 4, 6, "Анд'хакар, Чернильная тьма", "Этот щит принадлежал Мейкиду, рыцарю Престола Полумесяца, который поклялся убить кракена, чтобы отомстить за погибших в море сыновей. Он не вернулся из похода. Теперь этот щит наполнен темной силой морских глубин.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_shield_0055.png" },
                    { 4, 8, "Грозовое ожерелье", "Ожерелье, пропитанное силой ужасного Кракена. Не дай бог оно вам упадёт...", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_necklace_0016.png" },
                    { 4, 6, "Плащ проклятой души", "Много веков назад этот плащ принадлежал отважному мореходу, потерявшему жизнь в битве с Калидисом.", null, "http://localhost:11000/freaks/loot_items/icons/nu_m_cp_leather277.png" },
                    { 4, 6, "Нарвиг, огненный клинок Морфеоса", "«Когда огонь и лед сойдутся вместе, их ничто не остановит». - Морфеос Дважды Проклятый. Объединив Нарвиг, огненный клинок Морфеоса и Исбранд, ледяной клинок Марли на походном верстаке, можно получить Иснарнир, Ледяное пламя.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_blade_1h_0013.dds.png" },
                    { 4, 6, "Капюшон иферийского советника", "Лёгкий доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_hm_cloth185.png" },
                    { 4, 6, "Мантия иферийского советника", "Лёгкий доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_ar_cloth185.png" },
                    { 4, 6, "Перчатки иферийского советника", "Лёгкий доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_gv_cloth185.png" },
                    { 4, 6, "Поножи иферийского советника", "Лёгкий доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_pt_cloth185.png" },
                    { 4, 6, "Сапоги иферийского советника", "Лёгкий доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_bo_cloth185.png" },
                    { 4, 6, "Шлем любимца Изы", "Средний доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_m_hm_leather277.png" },
                    { 4, 6, "Доспех любимца Изы", "Средний доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_ar_leather277.png" },
                    { 4, 6, "Поножи любимца Изы", "Средний доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_pt_leather277.png" },
                    { 4, 6, "Перчатки любимца Изы", "Средний доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_gv_leather277.png" },
                    { 4, 6, "Сапоги любимца Изы", "Средний доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_bo_leather277.png" },
                    { 4, 6, "Шлем властелина морей", "Тяжелый доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_m_hm_metal179.png" },
                    { 4, 6, "Бригантина властелина морей", "Тяжелый доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_ar_metal179.png" },
                    { 4, 6, "Поножи властелина морей", "Тяжелый доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_m_pt_metal179.png" },
                    { 4, 6, "Перчатки властелина морей", "Тяжелый доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_m_gv_metal179.png" },
                    { 4, 6, "Сапоги властелина морей", "Тяжелый доспех.", null, "http://localhost:11000/freaks/loot_items/icons/nu_f_bo_metal179.png" },
                    { 5, 6, "Каменное сердце Морфеоса", "Можно получить, одержав победу над Морфеосом Дважды Проклятым.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_quest028.png" },
                    { 5, 6, "Средоточие ярости", "Трофей за победу над Калидисом. Необходим для изготовления свитков пробуждения снаряжения.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4772.png" },
                    { 5, 6, "Средоточие сумрака", "Трофей за победу над Ксанатосом. Необходим для изготовления свитков пробуждения снаряжения.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4771.png" },
                    { 5, 6, "Средоточие морей", "Трофей за победу над Левиафаном. Необходим для изготовления свитков пробуждения снаряжения.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4770.png" },
                    { 5, 6, "Средоточие безумия", "Трофей за победу над Анталлоном. Необходим для изготовления свитков пробуждения снаряжения.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4773.png" },
                    { 6, 6, "Аметистовая гравировка «Морского дракона»", "С некоторой вероятностью можно получить, одолев капитана Гленна или заклинательницу Лорею.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4190.png" },
                    { 6, 6, "Аметистовая гравировка багровой грозы", "С некоторой вероятностью можно получить, одолев Левиафана, Гартарейн или Калидиса.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4190.png" },
                    { 6, 6, "Аметистовая гравировка кровавой длани", "С некоторой вероятностью можно получить, одолев Анталлона или Сехекмет.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4190.png" },
                    { 6, 6, "Аметистовая гравировка морской бездны", "С некоторой вероятностью можно получить, одолев Кракена.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4190.png" },
                    { 6, 6, "Аметистовая гравировка пиратской метки", "С некоторой вероятностью можно получить, одолев Морфеоса Дважды Проклятого или капитана Марли.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4190.png" },
                    { 6, 6, "Аметистовая гравировка северной звезды", "С некоторой вероятностью можно получить, одолев Авиару.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4190.png" },
                    { 7, 6, "Свиток пробуждения нуонского драконоборца", "С помощью этого свитка можно пробудить пламенеющее снаряжение из Логова дракона (качества эпохи легенд и выше).", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4523.png" },
                    { 8, 6, "Генетический материал дракона", "Если АПОКАЛИПСИС не врет, очень скоро у вас появится свой собственный дракон! Отправляйтесь к агенту в Мэрианхольд, Ост-Терру или на Остров свободы.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4260.png" },
                    { 9, 2, "Глайдер «Рассекатель небес»", "Этому глайдеру не страшны переменчивый ветер и турбулентные потоки.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_1369.png" },
                    { 9, 2, "Глайдер «Властелин морей»", "Этому глайдеру не страшны переменчивый ветер и турбулентные потоки.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_1205.png" },
                    { 10, 6, "Золото", "Золото: внутриигровая валюта.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_0191.dds.png" },
                    { 30, 8, "Кристалл властелина морей", "Используется для изготовления снаряжения властелина морей.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4768.png" },
                    { 30, 6, "Эссенция кошмара", "Ремесленный материал. Требуется для изготовления транспортного оборудования.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_5002.png" },
                    { 30, 6, "Эссенция гнева", "Ремесленный материал. Требуется для изготовления транспортного оборудования.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_5001.png" },
                    { 30, 6, "Эссенция ужаса", "Ремесленный материал. Требуется для изготовления транспортного оборудования.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_5003.png" },
                    { 30, 7, "Чернильный мешочек Кракена", "Можно получить, одержав победу над Кракеном. Используется в ремесле.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_1961.png" },
                    { 30, 7, "Лоскут кожи Калидиса", "Серая кожа древнего чудовища покрыта пучками колкой щетины. Требуется для изготовления снаряжения любимца Изы.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4813.png" },
                    { 30, 8, "Клык Калидиса", "Гнилой желтоватый клык. Требуется для изготовления снаряжения любимца Изы.", null, "http://localhost:11000/freaks/loot_items/icons/icon_item_4812.png" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

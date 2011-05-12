$(document).ready(function () {

    if (!glimpse)
        glimpse = '', glimpsePath = '/';

    var scenarioOneData = {
        'Movie': 'Star Wars',
        'Movie1': '[hi] [byte]',
        'Movie2': '![hi] [byte]!',
        'Genera/Theme': 'Science Fiction',
        'GlimpseOn': 'True',
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-one').show().html($.glimpseProcessor.build(scenarioOneData, 0));

    var scenarioTwoData = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', 'quiet'],
        ['Harrison Ford', 'Han Solo', 'Male', '25'],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
        ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', 'selected'],
        ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
        ['Kenny Baker', 'R2-D2', 'Droid', '150']
    ];
    $('.glimpse-scenario-two').show().html($.glimpseProcessor.build(scenarioTwoData, 0));

    var scenarioTwoDataA = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', 'info'],
        ['Harrison Ford', 'Han Solo - Solo plays a central role in the various Star Wars set after Return of the Jedi. In The Courtship of Princess Leia (1995), he resigns his commission to pursue Leia, whom he eventually marries.', 'Male', '25'],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
        ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', 'warn'],
        ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
        ['Kenny Baker', 'R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.', 'Droid', '150'],
        ['Peter Mayhew', 'Chewbacca', 'Wookie', '45'],
        ['Phil Brown', 'Uncle Owen', 'Male', '32', 'error'],
        ['Shelagh Fraser', 'Aunt Beru', 'Female', '29'],
        ['Alex McCrindle', 'General Dodonna', 'Male', '43', 'fail']
    ];
    $('.glimpse-scenario-two-a').show().html($.glimpseProcessor.build(scenarioTwoDataA, 0));

    var scenarioThreeData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-three').show().html($.glimpseProcessor.build(scenarioThreeData, 0));


    var scenarioThreeDataA = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker - Luke Skywalker was a legendary war hero and Jedi who helped defeat the Galactic Empire in the Galactic Civil War and helped found the New Republic, as well as the New Jedi Order.', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-three-a').show().html($.glimpseProcessor.build(scenarioThreeDataA, 0));

    var scenarioFourData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-four').show().html($.glimpseProcessor.build(scenarioFourData, 0));

    var scenarioFourDataA = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker - Luke Skywalker was a legendary war hero and Jedi who helped defeat the Galactic Empire in the Galactic Civil War and helped found the New Republic, as well as the New Jedi Order.', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-four-a').show().html($.glimpseProcessor.build(scenarioFourDataA, 0));

    var scenarioFiveData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo'
        },
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-five').show().html($.glimpseProcessor.build(scenarioFiveData, 0));

    var scenarioSixData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo',
            'Carrie Fisher': 'Princess Leia Organa',
            'Peter Cushing': 'Grand Moff Tarkin',
            'Alec Guinness': 'Ben Obi-Wan Kenobi',
            'Anthony Daniels': 'C-3PO',
            'Kenny Baker': 'R2-D2'
        },
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };

    $('.glimpse-scenario-six').show().html($.glimpseProcessor.build(scenarioSixData, 0));

    var scenarioSevenData = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
            ], 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];

    $('.glimpse-scenario-seven').show().html($.glimpseProcessor.build(scenarioSevenData, 0));

    var scenarioEightData = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ], 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];

    $('.glimpse-scenario-eight').show().html($.glimpseProcessor.build(scenarioEightData, 0));

    var scenarioNineData = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo'
        }, 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];

    $('.glimpse-scenario-nine').show().html($.glimpseProcessor.build(scenarioNineData, 0));

    var scenarioTenData = [
        ['Type', 'Name', 'Other'],
        ['Movie', 'Star Wars', 'Episode IV'],
        ['Genera/Theme', 'Science Fiction', 'Action'],
        ['Actors/Cast', {
            'Mark Hamill': 'Luke Skywalker',
            'James Earl Jones': 'Darth Vader',
            'Harrison Ford': 'Han Solo',
            'Carrie Fisher': 'Princess Leia Organa',
            'Peter Cushing': 'Grand Moff Tarkin',
            'Alec Guinness': 'Ben Obi-Wan Kenobi',
            'Anthony Daniels': 'C-3PO',
            'Kenny Baker': 'R2-D2'
        }, 'Cast details pending'],
        ['Plot & Description', 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.', ' Best movie of all time']
    ];

    $('.glimpse-scenario-ten').show().html($.glimpseProcessor.build(scenarioTenData, 0));

    var scenarioElevenData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', { 'Frist Name': 'Luke Skywalker' }, 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', [['Height', 'Weight'], ['213', '433']], 'Male'],
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-eleven').show().html($.glimpseProcessor.build(scenarioElevenData, 0));

    var scenarioTwelveData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', { 'Frist Name': 'Luke Skywalker' }, 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', [['Height', 'Weight'], ['213', '433']], 'Male'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'],
                ['Peter Cushing', 'Grand Moff Tarkin', 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-twelve').show().html($.glimpseProcessor.build(scenarioTwelveData, 0));

    var scenarioThirteenData = {
        'Movie': [ 'Star Wars', 'Star Treck'],
        'Genera/Theme': [ 'Science Fiction', 'Drama', 'Romance', 'Action', 'Family' ],
        'Actors/Cast': [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', [ 'Luke Skywalker', 'Mini Skywalker' ], 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', [['Height', 'Weight'], ['213', '433'], ['123', '45']], 'Male'],
                ['Harrison Ford', 'Han Solo', 'Male', '25'],
                ['Carrie Fisher', [ 'Princess Leia Organa' ], 'Female', '21'],
                ['Peter Cushing', [ 'Grand Moff Tarkin', 'Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin Grand Moff Tarkin' ], 'Female', '69'],
                ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70'],
                ['Anthony Daniels', 'C-3PO', 'Droid', '101'],
                ['Kenny Baker', 'R2-D2', 'Droid', '150']
            ],
        'Plot & Description': [ 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.' ]
    };
    $('.glimpse-scenario-thirteen').show().html($.glimpseProcessor.build(scenarioThirteenData, 0));
     
    if (glimpse == '') 
        $.glimpse._wireCommonPluginEvents($.glimpse); 
    $.glimpseProcessor.applyPostRenderTransforms($('.protocol'));
});
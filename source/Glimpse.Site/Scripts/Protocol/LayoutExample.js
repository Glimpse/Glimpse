

if (!document.glimpse)
    glimpse = '', glimpsePath = '/Glimpse.axd?r=';

$(document).ready(function () {


    var scenarioFourteenData = [
        ['Actor', 'Character', 'Gender', 'Age', 'Height'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', '1' ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', '4', 'info' ],
        ['Harrison Ford', 'Han Solo - Solo plays a central role in the various Star Wars set after Return of the Jedi. In The Courtship of Princess Leia (1995), he resigns his commission to pursue Leia, whom he eventually marries.', 'Male', '25', '9'],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21', '8'],
        ['Peter Cushing',  [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ], 'Female', '69', '1'],
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70', '9' ],
        ['Anthony Daniels',  [
                ['Actor', 'Character', 'Gender', 'Age'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. Darth Vader. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. Darth Vader. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker.', 'Male', '45'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45'],
                ['Mark Hamill', 'Luke Skywalker', 'Male', '21'],
                ['James Earl Jones', 'Darth Vader', 'Male', '45']
            ], 'Droid', '101', '2'],
        ['Kenny Baker', 'R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message. R2-D2 - R2-D2 and C-3PO are abducted by Jawas and bought by Owen Lars, step-uncle of Luke Skywalker. While Luke cleans the sand out of R2-D2\'s gears, he discovers a fragment of Leia\'s message, and removes the droid\'s restraining bolt to see more; once free of the bolt, R2 claims to have no knowledge of the message.', 'Droid', '150', '6']
    ]; 
    var scenarioFourteenMetadata = [
            [ { data : [{ data : 0, key : true, align : 'right' }, { data : 2, align : 'right' }, { data : '{{3}} - {{4}}', align : 'right' }, ], width : '200px' }, { data : 1 } ]
        ];
    $('.glimpse-scenario-fourteen').show().html($Glimpse.glimpseProcessor.build(scenarioFourteenData, 0, scenarioFourteenMetadata));
 

    
    var scenarioFifteenData = [
        ['Actor', 'Gender', 'Age', 'Height', 'Character'],
        ['Mark Hamill', 'Male', '21', '2', '-- Search for Cataclysmic Variables and pre-CVs with White Dwarfs and \r\n-- very late secondaries. Just uses some simple color cuts from Paula Szkody.  \r\n-- Another simple query that uses math in the WHERE clause  \r\n\r\nSELECT run, \r\n\tcamCol, \r\n\trerun, \r\n\tfield, \r\n\tobjID, \r\n \tra -- Just get some basic quantities \r\nFROM PhotoPrimary	 -- From all primary detections, regardless of class \r\nWHERE u - g < 0.4 \r\n\tand g - r < 0.7 \r\n\tand r - i > 0.4 \r\n\tand i - z > 0.4 -- that meet the color criteria' ],
        ['James Earl Jones', 'Male', '45', '5', '-- Low-z QSO candidates using the color cuts from Gordon Richards. \r\n-- Also a simple query with a long WHERE clause. \r\n\r\nSELECTg,\r\n\trun,\r\n\trerun,\r\n\tcamcol,\r\n\tfield,\r\n\tobjID\r\nFROMGalaxy\r\nWHERE ( (g <= 22)\r\n\tand (u - g >= -0.27)\r\n\tand (u - g < 0.71)\r\n\tand (g - r >= -0.24)\r\n\tand (g - r < 0.35)\r\n\tand (r - i >= -0.27)\r\n\tand (r - i < 0.57)\r\n\tand (i - z >= -0.35)\r\n\tand (i - z < 0.70) )' ],
        ['Carrie Fisher', 'Female', '21', '3', '-- Using object counting and logic in a query. \r\n-- Find all objects similar to the colors of a quasar at 5.5 \r\n\r\nSELECT count(*) as \'total\',\r\n\tsum( case when (Type=3) then 1 else 0 end) as \'Galaxies\',\r\n\tsum( case when (Type=6) then 1 else 0 end) as \'Stars\',\r\n\tsum( case when (Type not in (3,6)) then 1 else 0 end) as \'Other\'\r\nFROM PhotoPrimary	 -- for each object\r\nWHERE (( u - g > 2.0) or (u > 22.3) ) -- apply the quasar color cut.\r\n\tand ( i between 0 and 19 )\r\n\tand ( g - r > 1.0 )\r\n\tand ( (r - i < 0.08 + 0.42 * (g - r - 0.96)) or (g - r > 2.26 ) )\r\n\tand ( i - z < 0.25 )' ], 
        ['Alec Guinness', 'Female', '70', '9', '-- This is a query from Robert Lupton that finds selected neighbors in a given run and \r\n-- camera column. It contains a nested query containing a join, and a join with the \r\n-- results of the nested query to select only those neighbors from the list that meet \r\n-- certain criteria. The nested queries are required because the Neighbors table does \r\n-- not contain all the parameters for the neighbor objects. This query also contains \r\n-- examples of setting the output precision of columns with STR. \r\n\r\nSELECT\r\n\tobj.run, obj.camCol, str(obj.field, 3) as field,\r\n\tstr(obj.rowc, 6, 1) as rowc, str(obj.colc, 6, 1) as colc,\r\n\tstr(dbo.fObj(obj.objId), 4) as id,\r\n\tstr(obj.psfMag_g - 0*obj.extinction_g, 6, 3) as g,\r\n\tstr(obj.psfMag_r - 0*obj.extinction_r, 6, 3) as r,\r\n\tstr(obj.psfMag_i - 0*obj.extinction_i, 6, 3) as i,\r\n\tstr(obj.psfMag_z - 0*obj.extinction_z, 6, 3) as z,\r\n\tstr(60*distance, 3, 1) as D,\r\n\tdbo.fField(neighborObjId) as nfield,\r\n\tstr(dbo.fObj(neighborObjId), 4) as nid,\'new\' as \'new\' \r\nFROM\r\n\t\t(SELECT obj.objId,\r\n\t\t\trun, camCol, field, rowc, colc,\r\n\t\t\tpsfMag_u, extinction_u,\r\n\t\t\tpsfMag_g, extinction_g,\r\n\t\t\tpsfMag_r, extinction_r,\r\n\t\t\tpsfMag_i, extinction_i,\r\n\t\t\tpsfMag_z, extinction_z,\r\n\t\t\tNN.neighborObjId, NN.distance\r\n\t\tFROM PhotoObj as obj\r\n\t\t\tJOIN neighbors as NN on obj.objId = NN.objId\r\n\t\tWHERE 60*NN.distance between 0 and 15 and\r\n\t\t\tNN.mode = 1 and NN.neighborMode = 1 and\r\n\t\t\trun = 756 and camCol = 5 and\r\n\t\t\tobj.type = 6 and (obj.flags & 0x40006) = 0 and\r\n\t\t\tnchild = 0 and obj.psfMag_i < 20 and\r\n\t\t\t(g - r between 0.3 and 1.1 and r - i between -0.1 and 0.6)\r\n\t\t) as obj \r\nJOIN PhotoObj as nobj on nobj.objId = obj.neighborObjId \r\nWHERE\r\n\tnobj.run = obj.run and\r\n\t(abs(obj.psfMag_g - nobj.psfMag_g) < 0.5 or\r\n\tabs(obj.psfMag_r - nobj.psfMag_r) < 0.5 or\r\n\tabs(obj.psfMag_i - nobj.psfMag_i) < 0.5) \r\nORDER BY obj.run, obj.camCol, obj.field ' ]
    ]; 
    var scenarioFifteenMetadata = [
            [ { data : [ { data : 0, key : true, align : 'right' }, { data : 1, align : 'right' }, { data : 2, align : 'right', post : ' ms', className : 'mono' }, { data : 3, align : 'right', pre : 'T+ ', post : ' ms', className : 'mono' } ], width : '200px' }, { data : 4, isCode : true, codeType : 'sql' } ]
        ];
    $('.glimpse-scenario-fifteen').show().html($Glimpse.glimpseProcessor.build(scenarioFifteenData, 0, scenarioFifteenMetadata));





    
    var scenarioSixteenData = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21' ],
        ['James Earl Jones', 'Darth Vader', 'Male', '45', 'info' ],
        ['Carrie Fisher', 'Princess Leia Organa', 'Female', '21'], 
        ['Alec Guinness', 'Ben Obi-Wan Kenobi', 'Female', '70' ]
    ];
    var scenarioSixteenMetadata = [
            [ { data : '{{0}} - ({{1}})', key : true }, { data : 2, width : '20%' }, { data : 3, width : '20%' } ]
        ];
    $('.glimpse-scenario-sixteen').show().html($Glimpse.glimpseProcessor.build(scenarioSixteenData, 0, scenarioSixteenMetadata));











    var scenarioOneData = {
        'Movie': 'Star Wars',
        'Genera/Theme': 'Science Fiction',
        'GlimpseOn': 'True',
        'Plot & Description': 'Luke Skywalker leaves his home planet, teams up with other rebels, and tries to save Princess Leia from the evil clutches of Darth Vader.'
    };
    $('.glimpse-scenario-one').show().html($Glimpse.glimpseProcessor.build(scenarioOneData, 0));

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
    $('.glimpse-scenario-two').show().html($Glimpse.glimpseProcessor.build(scenarioTwoData, 0));

    var scenarioTwoDataA = [
        ['Actor', 'Character', 'Gender', 'Age'],
        ['Mark Hamill', 'Luke Skywalker', 'Male', '21', 'ms'],
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
    $('.glimpse-scenario-two-a').show().html($Glimpse.glimpseProcessor.build(scenarioTwoDataA, 0));

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
    $('.glimpse-scenario-three').show().html($Glimpse.glimpseProcessor.build(scenarioThreeData, 0));


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
    $('.glimpse-scenario-three-a').show().html($Glimpse.glimpseProcessor.build(scenarioThreeDataA, 0));

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
    $('.glimpse-scenario-four').show().html($Glimpse.glimpseProcessor.build(scenarioFourData, 0));

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
    $('.glimpse-scenario-four-a').show().html($Glimpse.glimpseProcessor.build(scenarioFourDataA, 0));

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
    $('.glimpse-scenario-five').show().html($Glimpse.glimpseProcessor.build(scenarioFiveData, 0));

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

    $('.glimpse-scenario-six').show().html($Glimpse.glimpseProcessor.build(scenarioSixData, 0));

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

    $('.glimpse-scenario-seven').show().html($Glimpse.glimpseProcessor.build(scenarioSevenData, 0));

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

    $('.glimpse-scenario-eight').show().html($Glimpse.glimpseProcessor.build(scenarioEightData, 0));

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

    $('.glimpse-scenario-nine').show().html($Glimpse.glimpseProcessor.build(scenarioNineData, 0));

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

    $('.glimpse-scenario-ten').show().html($Glimpse.glimpseProcessor.build(scenarioTenData, 0));

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
    $('.glimpse-scenario-eleven').show().html($Glimpse.glimpseProcessor.build(scenarioElevenData, 0));

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
    $('.glimpse-scenario-twelve').show().html($Glimpse.glimpseProcessor.build(scenarioTwelveData, 0));

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
    $('.glimpse-scenario-thirteen').show().html($Glimpse.glimpseProcessor.build(scenarioThirteenData, 0));
     
    if (glimpse == '') 
        $Glimpse.glimpse._wireCommonPluginEvents($.glimpse); 
    $Glimpse.glimpseProcessor.applyPostRenderTransforms($('.protocol'));


    //Sample rendering
    var sampleRender = function(data) {
        $('.glimpse-scenario-example').show().html($Glimpse.glimpseProcessor.build(data, 0));
        $Glimpse.glimpseProcessor.applyPostRenderTransforms($('.glimpse-scenario-example'));
    }
    sampleRender(scenarioOneData);

    //Trigger new rendering
    $('.glimpse-sample-trigger').click(function(e) {
        e.preventDefault;
        try
        {
            var data = jQuery.parseJSON($('.glimpse-sample').val()); 
            sampleRender(data); 
        }
        catch(err)
        {
            alert(err);
        }

        return false;
    });
});
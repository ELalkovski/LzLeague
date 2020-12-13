$('#groups-select-results').change(function () {

    let selectedValue = $(this).find('option:selected').val();

    if (selectedValue.length === 1) {
        $.ajax({
            url: '/Admin/Matches/GetAvailableMatchesByGroup',
            method: 'GET',
            data: { 'groupId': Number(selectedValue) }
        }).then(function (matches) {
            console.log(matches);

            $('#available-matches').empty();

            $('#available-matches').append($('<option>')
                .text('Please select match...')
                .prop('selected', 'selected'));

            for (let matchIndex in matches) {

                let currentMatch = matches[matchIndex];
                console.log('match');
                console.log(currentMatch);

                if (currentMatch) {
                    let matchTitle = currentMatch.homeTeamName + ' - ' + currentMatch.awayTeamName;
                    let currentOption = $('<option>')
                        .text(matchTitle)
                        .val(currentMatch.id);

                    $('#available-matches').append($(currentOption));
                }


            }

            $('#available-matches').removeAttr('disabled');
        });
    } else {

        $('#available-matches').empty();
        $('#available-matches').append($('<option>').text('Please select a group, so the games could be loaded.'));
        $('#available-matches').prop('disabled', 'disabled');
    }
});
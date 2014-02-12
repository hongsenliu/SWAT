
function pplPerHH()
{
    var population = document.getElementById('Population').value;
    var households = document.getElementById('numHouseholds').value;

    if (population != null && population.trim() != "" && households != null && households.trim() != "" && households > 0 && population > 0)
    {	
        document.getElementById('PeoplePerHH').disabled = true;
        document.getElementById('PeoplePerHH').value = population / households;
    }
    else
    {
        document.getElementById('PeoplePerHH').value = '';
        document.getElementById('PeoplePerHH').disabled = false;
    }
}

function chkAreaPercent()
{
    var forest = parseFloat(document.getElementById('admintblCommunityareaForest').value);
    var ag = parseFloat(document.getElementById('admintblCommunityareaAg').value);
    var areainf = parseFloat(document.getElementById('admintblCommunityareaInf').value);
    var sw = parseFloat(document.getElementById('admintblCommunityareaSW').value);
    var wet = parseFloat(document.getElementById('admintblCommunityareaWet').value);
    var nat = parseFloat(document.getElementById('admintblCommunityareaNat').value);
    var prot = parseFloat(document.getElementById('admintblCommunityareaProt').value);
    var bestm = parseFloat(document.getElementById('admintblCommunityareaBM').value);
    if(prot < 0 || prot > 100)
    {
        document.getElementById('admintblCommunityareaProt').style.backgroundColor="red";
        alert("Percentage of land under protected has to be >= 0 and <= 100.");
    }
    else if(nat < 0 || nat > 100)
    {
        document.getElementById('admintblCommunityareaNat').style.backgroundColor="red";
        alert("Percentage of native or natural lands has to be >= 0 and <= 100.");
    }
    else if(wet < 0 || wet > 100)
    {
        document.getElementById('admintblCommunityareaWet').style.backgroundColor="red";
        alert("Percentage of wetlands has to be >= 0 and <= 100.");
    }
    else if(sw < 0 || sw > 100)
    {
        document.getElementById('admintblCommunityareaSW').style.backgroundColor="red";
        alert("Percentage of source water has to be >= 0 and <= 100.");
    }
    else if(areainf < 0 || areainf > 100)
    {
        document.getElementById('admintblCommunityareaInf').style.backgroundColor="red";
        alert("Percentage of infrastructure has to be >= 0 and <= 100.");
    }
    else if(ag < 0 || ag > 100)
    {
        document.getElementById('admintblCommunityareaAg').style.backgroundColor="red";
        alert("Percentage of agriculture has to be >= 0 and <= 100.");
    }
    else if(forest < 0 || forest > 100)
    {
        document.getElementById('admintblCommunityareaForest').style.backgroundColor="red";
        alert("Percentage of forest has to be >= 0 and <= 100.");
    }
    else if(bestm < 0 || bestm > 100)
    {
        document.getElementById('admintblCommunityareaBM').style.backgroundColor="red";
        alert("Percentage of under best management practices has to be >= 0 and <= 100.");
    }
    else
    {
        if((forest + ag + areainf + sw + wet) > 100)
        {
            document.getElementById('admintblCommunityareaForest').style.backgroundColor="red";
            document.getElementById('admintblCommunityareaAg').style.backgroundColor="red";
            document.getElementById('admintblCommunityareaInf').style.backgroundColor="red";
            document.getElementById('admintblCommunityareaSW').style.backgroundColor="red";
            document.getElementById('admintblCommunityareaWet').style.backgroundColor="red";
            alert("Sum of percentages in Forest, Agriculture, Infrastructure, Source Water and Wetlands cannot be over 100.");
        }
        else
        {
            document.getElementById('admintblCommunityareaForest').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaAg').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaInf').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaSW').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaWet').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaNat').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaProt').style.backgroundColor="white";
            document.getElementById('admintblCommunityareaBM').style.backgroundColor="white";
        }
    }
	
}
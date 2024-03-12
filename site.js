
$(document).ready(function(){
    $("#error").hide();
    $("#main").show();
})


$("#email").keyup(function(){
    debugger;
    var reg=/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if(!reg.test($("#email").val()))
   {
    $("#error").show();
    $("#submit").prop('disabled',true);
   }
   else{
    $("#error").hide();
    $("#submit").prop('disabled',false);
   }

})

var currentIndex = 0;
options=[]
ans=[]
$("#submit").click(function(){
    var email=$("#email").val();
    $.ajax({  
        type: "POST",  
        url: '/Home/Submit',  
        data: {email:email},  
        dataType: "json",
        async: true,
        success: function (_data) {  
            $("#section").hide();
            $("#main").show(); 
            $("#next").prop("disabled",true);
            displayQuestion(_data);
            
        },  
        error: function () {  
            alert("Error while inserting data");  
        }  
    }); 
    
})


$("#next").click(function() {
    options.push($("input[name='radio']:checked").next('label').text());
    if (hasNextQuestion(_data)) {
        currentIndex++;
        displayQuestion(_data);
    } else {
       
    }
});

function displayQuestion(_data) {
    var _this = _data[currentIndex];
    ans.push(_this.Answer);
       $("input[type='question'][value='0']").next().text(''+_this.Question+'');
       var control=_this.Options.split(",");
       $("input[type='radio']").each(function(i) {
           $(this).next('label').text(control[i]);
       });
       $("#next").prop("disabled",false);
     
}

function hasNextQuestion(data) {
    return currentIndex < data.length - 1;
}
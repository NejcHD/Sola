window.onload = function() {
    consloe.log("Prikaz objav");
    
    const container = document.getElementById("objave-container");
    
    // 1 preverimo ce objava obstaja
    if(!VseObjave || VseObjave.length === 0){
        container.innerHTML = "<p> Ni Objave </p>";
        return;
    }
    
    
    // pregled vsake objave
    vseObjave.foreach(function(objave, index) {
        
        const dan = objava.datum.getDate().toString().padStart(2, "0");
        const mesec = (objave.datum.getMonth() + 1).toString().padStart(2, "0");
        const leto  = objava.datum.GetFullyear();
        
        
        
        //DataTemplate za izpis pdoatko v html
        const html = 
            <div class="objava">
                <div style="display: flex;">
                    <div style="width: 90px">
                        
            </div>
    });
}
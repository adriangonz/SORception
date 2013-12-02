module.directive( "requestTokenButton", [ 'Scrap', function( Scrap ) {
   return {
     restrict: "A",
     scope: {
      config : '=requestTokenButton'
     },
     link: function( scope, element, attrs ) {
       element.bind( "click", function() {
         //scope.config -> .name & .token (old);

        $.ajax({
          url: 'URL_SERVER/token',
          type: 'POST',
          data: scope.config,
          success: function(){ 
            alert('success!'); //Ni idea como hacer el redirect
          },
          error: function(){
            alert('error!'); //Deberia mostrar una alerta bonita
          }
        });

       });
     }
   }
 }]);

module.directive( "addPartButton", [ 'Scrap', function( Scrap ) {
   return {
     restrict: "A",
     scope: {
      part : '=addPartButton'
     },
     link: function( scope, element, attrs ) {
       element.bind( "click", function() {
         Scrap.addPart( scope.part );
       });
     }
   }
 }]);
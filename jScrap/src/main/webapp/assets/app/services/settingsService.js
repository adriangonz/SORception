module.service( 'SettingsService', [ '$rootScope', '$http', function( $rootScope, $http ) {
   var service = {
	    settings: {
	     	"name": "",
	     	"validToken":{},
	     	"tokenList": {},
	     	"userList": {},
	 		},
	 
	    getSettings: function () {
	        $http({method: 'GET', url: '/jScrap/api/settings'}).
			  success(function(data, status, headers, config) {
	       		service.settings=data;
	       		$rootScope.$broadcast( 'settings.update' );
			  }).
			  error(function(data, status, headers, config) {
			  	alert(status+" | "+data);
			  	service.settings.name="DesguaceGET";
			  	service.settings.tokenList = [
				   {
				        "id": 1,
				        "token": "TOKEN_1",
				        "status": "REQUESTED", // 'REQUESTED', 'VALID' o 'EXPIRED'
				        "creationDate": "FECHA_1"
				   },
				   {
				        "id": 2,
				        "token": "TOKEN_2",
				        "status": "EXPIRED", // 'REQUESTED', 'VALID' o 'EXPIRED'
				        "creationDate": "FECHA_2"
				   },
				   {
				        "id": 3,
				        "token": "TOKEN_3",
				        "status": "VALID", // 'REQUESTED', 'VALID' o 'EXPIRED'
				        "creationDate": "FECHA_4"
				   }
				];
	       		$rootScope.$broadcast( 'settings.update' );
			  });    	
	    },

	    postSettings: function(){
	     	  $http({method: 'POST', url: '/jScrap/api/token'}).
	          success(function(data, status, headers, config) {
	            service.getSettings();
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },

	    getUsers: function () {
	        $http({method: 'GET', url: '/jScrap/api/user'}).
			  success(function(data, status, headers, config) {
	       		service.settings.userList=data;
	       		$rootScope.$broadcast( 'settings.update' );
			  }).
			  error(function(data, status, headers, config) {
			  	alert(status+" | "+data);
			  	service.settings.userList = [
				   {
				        "id": 1,
				        "username": "Admin",
				        "name": "su nombre",
				        "isAdmin": true,
				        "creationDate": "FECHA_1"
				   },
				   {
				        "id": 2,
				        "username": "Other",
				        "name": "su nombre",
				        "isAdmin": false,
				        "creationDate": "FECHA_2"
				   },
				   {
				        "id": 3,
				        "username": "Other2",
				        "name": "su nombre",
				        "isAdmin": false,
				        "creationDate": "FECHA_4"
				   }
				];
	       		$rootScope.$broadcast( 'settings.update' );
			  });    	
	    },

	   	postUser: function(user_data){
	     	  $http({method: 'POST', url: '/jScrap/api/user', data: user_data}).
	          success(function(data, status, headers, config) {
	            service.getUsers();
	          }).
	          error(function(data, status, headers, config) {
	            alert(status+" | "+data);
	          });
	    },
   	}
 
   return service;
}]);


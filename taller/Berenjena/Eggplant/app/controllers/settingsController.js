module.controller("SettingsCtrl", ['$scope', 'SettingsService', 'Auth', function ($scope, SettingsService, Auth) {
	$scope.$on( 'settings.update', function( event ) {
		$scope.settings = SettingsService.settings;
	});
	$scope.$on('userList.update', function (event) {
	    $scope.userList = SettingsService.userList;
	});

	Auth.isLoggedIn();
	Auth.isPrivate();

	SettingsService.getSettings();
	SettingsService.getUsers();
  	$scope.settings;

}]);

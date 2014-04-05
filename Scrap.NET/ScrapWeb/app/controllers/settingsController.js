module.controller( "SettingsCtrl", [ '$scope', 'SettingsService','Auth', function( $scope, SettingsService, Auth ) {
    $scope.$on('settings.update', function (event) {
        $scope.settings = SettingsService.settings;
    });
    $scope.$on('userList.update', function (event) {
        $scope.userList = SettingsService.userList;
    });
    $scope.$on('logs.update', function (event) {
        $scope.logs = SettingsService.logs;
    });

    console.log("Adri es un pesao");

	
    Auth.isLoggedIn();
    Auth.isPrivate();
	
	SettingsService.getSettings();
	SettingsService.getUsers();
	SettingsService.getLogs();
  	$scope.settings;

}]);

app.controller('ctrl', ['$scope','$http', function($scope,$http) {
  FnNcc($scope, $http);
}]);

function FnNcc($scope, $http) {
  $scope.ifShow = false;
  $scope.onCreate = function () {
    $scope.infoEvent = "Thêm Nhà Cung Cấp Mới";
    $('.modal-createedit').modal();
    $scope.ifShow = !$scope.ifShow;
    $scope.TenNcc = "";
    $scope.MasoThue = "";
    $scope.DienThoai = "";
    $scope.DiaChi = "";
    $scope.DanhMucHang = "";
    $scope.NgayGhi = new Date();
    $scope.Loai = "NCC";
    $scope.IdCty = "0";
    $scope.TenNv = "";
    $scope.Id = 0;
  };
  $scope.getAll = function() {
    var url = '/Ncc/GetAll';
    $scope.nccs = [];
    $http.post(url).then(function(e) {
      $scope.nccs = e.data.result;
      //console.log(JSON.stringify($scope.nccs));
    });
  };
  $http.getId = function() {
    var url = '/Ncc/GetId';
    $http.post(url).then(function(e) {
      //alert(e.data.result);
      $scope.Loai = "NCC";
      $scope.Id = 0;
      $scope.idcty = e.data.result;

    });
  };
  $scope.onEdit = function (id) {
   
    var url = '/Ncc/Edit?id='+id;
    $http.post(url).then(function (e) {
    
      var t = e.data.result;
      $scope.TenNcc = t.tenNcc;
      $scope.MasoThue = t.masoThue;
      $scope.DienThoai = t.dienThoai;
      $scope.DiaChi = t.diaChi;
      $scope.DanhMucHang = t.danhMucHang;
      $scope.NgayGhi = new Date(t.ngayGhi);
      $scope.Loai = t.loai;
      $scope.IdCty = t.idCty;
      $scope.TenNv = t.tenNv;
      $scope.Id = t.id;
      //$scope.MasoThue = t.maSoThue;
      //console.log($scope.Id);
      $scope.infoEvent = "Cập Nhật : " + t.tenNcc;
    });
  };
  $scope.keypressNcc = function() {
    var data = $scope.TenNcc;
    //console.log(JSON.stringify(data));
    var url = "Ncc/CheckTenNcc?data="+data;
    $http.post(url).then(function (e) {
    console.log(JSON.stringify(e));
      var dt = e.data.result;

      if (dt == "1") {
        $scope.warring = "(Tên này đã có, xin nhập tên khác!)";
      } else {
        $scope.warring = "";
      }
    });
  }
  $scope.onSubmit = function (id) {
   
    $scope.submitted = true;
    var request = {
      TenNcc: $scope.TenNcc,
      MasoThue: $scope.MasoThue,
      DienThoai: $scope.DienThoai,
      DiaChi: $scope.DiaChi,
      DanhMucHang: $scope.DanhMucHang,
      NgayGhi: new Date($scope.NgayGhi),
      Loai: $scope.Loai,
      IdCty: $scope.IdCty,
      Id: $scope.Id,
      TenNv: $scope.TenNv
    };
    //console.log(JSON.stringify(request));
    var url = '/Ncc/CreateOrEdit/';
    $http.post(url, request).then(function (e) {
      createSnackbar(e.data.result);
      $scope.getAll();
      $('.modal-createedit').modal('hide');
    });
  };
  $scope.onDelete = function(id) {
    var url = '/Ncc/Delete?id='+id;
    $http.post(url).then(function (e) {
      createSnackbar(e.data.result);
      $scope.getAll();
      $('.modal-createedit').modal('hide');
    });
  }
  function init() {
    $scope.getAll();
    $http.getId();
   
  }

  init();
};
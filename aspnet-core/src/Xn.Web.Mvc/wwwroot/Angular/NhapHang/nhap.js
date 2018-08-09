﻿app.controller('ctrl', ['$scope','$http', function($scope,$http) {
  FnNhap($scope, $http);
}]);

function FnNhap($scope, $http) {
  $scope.l = [];
  $scope.nhaphang = 'Nhập Hàng';
  $scope.them = "Thêm";
  $scope.createi = true;
$scope.n = 1;
  $scope.nccs = [];
  $scope.onChoise = function () {
   //$scope.l = [];
    for (var i = 0; i < $scope.n; i++) {
     
      var item = {
        Id: $scope.Id,
        TenNcc: $scope.TenNcc,
        IdNcc: $scope.IdNcc,
        TenHang: $scope.TenHang,
        SoLuong: $scope.SoLuong,
        DonGiaMua: $scope.DonGiaMua,
        Dvt: $scope.Dvt,
        NgayGhi: $scope.NgayGhi,
        IdCty: $scope.IdCty,
        MaDonHang: $scope.MaDonHang,
        IsActive: 1,
        MaVt: $scope.MaVt,
        Vat: $scope.vat,
        GhiChu:$scope.GhiChu
      };
      $scope.l.push(item);
    }
    $scope.tong();
   
  }
  $scope.GetMaDh = function() {
    var url = 'Nhap/GetMaDh';
    $http.post(url).then(function (e) {
      $scope.MaDonHang1 = e.data.result;
      //$scope.MaDonHang = e.data.result;
      $scope.NgayGhi = new Date();
    });
    $scope.createi = true;
  }

  $scope.tong = function() {
  
    $scope.sum = 0;
    for (var i = 0; i < $scope.l.length; i++) {
      $scope.sum += $scope.l[i].SoLuong * $scope.l[i].DonGiaMua;
    }
    $scope.conlai = $scope.sum - $scope.thanhtoan;
  }
  $scope.GetTenNcc = function () {
    var url = 'Nhap/GetTenNcc';
    $http.post(url).then(function (e) {
      $scope.nccs = e.data.result;
      console.log(JSON.stringify($scope.nccs));
      $scope.TenNcc = $scope.nccs[0];
    });
  };

  $scope.xoa = function(index) {
    $scope.l.splice(index, 1);
  };

  $scope.xoaedit = function(id) {
    var url = 'Nhap/Xoaedit?id=' + id;
    $http.post(url).then(function(e) {
      init();
    });
  };

  $scope.getDvt = function() {
    var url = 'Nhap/Dvts';
    $http.post(url).then(function(e) {
      $scope.dvts = e.data.result;
      //console.log(JSON.stringify(e));
    });
  };
  $scope.getTenHangs = function() {
    var url = 'Nhap/GetTenHang';
    $http.post(url).then(function(e) {
      $scope.getTenHg = e.data.result;
      console.log(JSON.stringify($scope.getTenHg));
    });

  };
  $scope.details = function(madh) {
    var url = 'Nhap/Details?id=' + madh;
    console.log(url);
    $http.post(url).then(function (e) {
  
    });
  };
  $scope.getAll = function() {
    var url = 'Nhap/GetAll';
    $http.post(url).then(function(e) {
      var dt = e.data.result;
     
      $scope.getTenHg = [];
      $scope.dvts = [];
      var dupes = {};
      $.each(dt, function (i, el) {

        if (!dupes[el.tenHang]) {
          dupes[el.tenHang] = true;
          $scope.getTenHg.push(el.tenHang);
        }
        //if (!dupes[el.dvt])
        //{
        //  dupes[el.dvt] = true;
        //  $scope.dvts.push(el.dvt);
        //}
       
      });
    
      //console.log(JSON.stringify($scope.dvts));
    });
  };
  $scope.onChange = function() {  
    if ($scope.chonGiay == 4) {
      $scope.gWidth = '210mm';
      $scope.gHeight = '297mm';
    };
    if ($scope.chonGiay == 5) {
      $scope.gWidth = '148mm';
      $scope.gHeight = '210mm';
    }
    localStorage.setItem("w", $scope.gWidth);
    localStorage.setItem("h", $scope.gHeight);
  };
  $scope.onSubmit = function () {
    var url = 'Nhap/CreateOrEdit/';
    var request = [];
    for (var i = 0; i < $scope.l.length; i++) {
      if ($scope.l[i].TenHang != ' ') {
        $scope.l[i].MaDonHang = $scope.MaDonHang1;
        $scope.l[i].Vat = $scope.vat;
        $scope.l[i].TenNcc = $scope.TenNcc;
        request.push($scope.l[i]);}
    }


    var item2 = {
      Id: $scope.Id,
      TenNcc: $scope.TenNcc,
      //IdNcc:'',
      //TenHang: "",
      SoLuong: $scope.sum,
      DonGiaMua: $scope.thanhtoan,
      Dvt: 'dvt',
      MaDonHang: $scope.MaDonHang1,
      IsActive: 1,
      //NgayGhi: $scope.NgayGhi,
      //IdCty: $scope.IdCty,
     
     
      //MaVt: '00'
    };
    request.push(item2);
    //console.log(JSON.stringify(request));
    $http.post(url, request).then(function (e) {
      //var dt = e.data.result;
      //console.log(e);
      $scope.getNhapXuat();
      $scope.createi = true;
      //$scope.GetMaDh();
    });
    
  };
  // ------------------- detail
  $scope.getNhap = function (mdh) {
    var url = "Report/Index?mdh=" + mdh;
    $http.post(url).then(function (e) {
      window.location.href = "/Report";
      localStorage.setItem("mdh", mdh);
      $scope.nhaps = e.data.result;
    });
  };
  //-------------------- danh sach nhap xuat
  $scope.getNhapXuat = function() {
    var url = 'Nhap/GetNxs/';
    $http.post(url).then(function (e) {
      $scope.listnhap = e.data.result;
      //console.log(JSON.stringify($scope.listnhap));
    });
  }
  $scope.onDelete = function(id) {
    //console.log(id);
    var url = '/Nhap/OnDelete?id=' + id;
    $http.post(url).then(function(e) {
      //console.log(e);
      $scope.getNhapXuat();
    });
  };
  $scope.loadCty = function () {
    var url = "Report/CongTy";
    $http.post(url).then(function (e) {
      $scope.inforCty = e.data.result;
      
    });
  };
  $scope.onEdit = function (mdh) {
    $scope.createi = false;
  $scope.them =  $scope.nhaphang = 'Cập Nhật';
    //console.log(mdh);
    var url = '/Nhap/Edit1?mdh=' + mdh;
    $http.post(url).then(function (e) {
      $scope.l = [];
      var dt = e.data.result;
      //console.log(JSON.stringify(dt[0].ngayGhi));
      for (var i = 0; i < dt.length -1; i++) {
        var h = dt[i];
        var item = {
          Id:h.id,
          TenNcc: h.TenNcc,
          IdNcc: h.idNcc,
          TenHang: h.tenHang,
          SoLuong: h.soLuong,
          DonGiaMua: h.donGiaMua,
          Dvt: h.dvt,
          NgayGhi: h.ngayGhi,
          IdCty: h.idCty,
          MaDonHang: h.maDonHang,
          IsActive: 1,
          MaVt: h.maVt,
          Ngay:''
        };
        $scope.l.push(item);
      }
      //console.log(JSON.stringify(dt[dt.length - 1]));
      var h1 = dt[dt.length - 1];
      $scope.Id = h1.id;
      $scope.TenNcc = dt[0].tenNcc; 
      $scope.sum = h1.soLuong;
      $scope.thanhtoan = h1.donGiaMua;
      $scope.conlai = ($scope.sum - h1.donGiaMua);
      $scope.n = dt.length - 1;
      $scope.NgayGhi = new Date(dt[0].ngayGhi);
      $scope.MaDonHang1 = dt[0].maDonHang;
   
    });
  }
  $scope.print = function (mdh) {
    $scope.mauNhap = "Mẫu số :  02-VT (Ban hành theo QĐ 15/2006/QĐ-BTC ngày 20/03/2006 của Bộ Trưởng Bộ Tài Chính)";
    var url = '/Nhap/PrintMdh?mdh=' + mdh;
    $http.post(url).then(function (e) {
      //console.log(JSON.stringify(e.data.result));
      $scope.nhapsmdh = e.data.result;
      $scope.hide_show_print = "printMdh_show";
    });
  }
  function init() {
    $scope.loadCty();
    $scope.hide_show_print = 'printMdh_hide';
    if (typeof(Storage) !== "undefined") {
      $scope.gWidth = localStorage.getItem("w");
      $scope.gHeight = localStorage.getItem("h");
    } else {
      // Sorry! No Web Storage support..
    }
    $scope.names = ["Emil", "Tobias", "Linus"];
    $scope.GetMaDh();
    $scope.getAll();
    //$scope.getL();
    $scope.GetTenNcc();
    $scope.getNhapXuat();
    $scope.getDvt();
    $scope.getTenHangs(); 
    $scope.vats = [];
    for (var i = 1; i <= 20; i++) {
      $scope.vats.push(i + "%");
    }
    $scope.vat = $scope.vats[9];
    $scope.onChoise();
  }
  init();
};
<?php
    @$penguin_connect = new mysqli('localhost', 'velmont', 'ekdrmfl67', 'velmont');
    mysql_query("set names utf8");
    if(mysqli_connect_errno()){
        echo 'Error';
        exit;
    }

    $id = $_GET['id'];
    $password = $_GET['password'];
    $email_address = $_GET['email_address'];
    $nick_name = $_GET['nick_name'];
    $is_admin = $_GET['is_admin'];
    $regdate = $_GET['regdate'];
    $last_login = $_GET['last_login'];
    if(!$email_address)
        die("No email address.");
    $query = $penguin_connect->query("INSERT INTO  `velmont`.`penguin_member` (`member_srl` ,`email_address`,`password` ,`nick_name` ,`is_admin` ,`regdate` ,`last_login`) VALUES (NULL, '$email_address', '$password', '$nick_name', '$is_admin', '$regdate', '$last_login')");
    if($query)
        echo 'success';
    $penguin_connect->close();
?>
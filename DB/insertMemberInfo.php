<?php
    @$penguin_connect = new mysqli('localhost', 'velmont', 'ekdrmfl67', 'velmont');
    mysql_query("set names utf8");
    if(mysqli_connect_errno()){
        echo 'Error';
        exit;
    }

    $email_address = $_POST['email_address'];
    $password = $_POST['password'];
    $nick_name = $_POST['nick_name'];
    $is_admin = $_POST['is_admin'];
    $regdate = $_POST['regdate'];
    $last_login = "1";
    if(!$id)
        die("No id");
    if(!$email_address)
        die("No email address.");
    if(!$password)
        die ("No password");
    if(!$nick_name)
        die("No nickname");
    if(!$is_admin)
        die("No is_admin");
    if(!$regdate)
        die("No regdate");
    $query = $penguin_connect->query("INSERT INTO `velmont`.`penguin_member` (`member_srl`, `id` ,`password` ,`nick_name` ,`email_address`,`is_admin` ,`regdate` ,`last_login`) VALUES (NULL, '$id','$password', '$nick_name',  '$email_address', '$is_admin', '$regdate', '$last_login')");
    if($query)
        echo "success";
    else
        echo "failed";
    $penguin_connect->close();
?>
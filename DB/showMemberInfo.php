<?php
    include('connectDB.php');
    $id = $_POST['id'];
    $query = $penguin_connect->query("SELECT * from  `velmont`.`penguin_member` WHERE id='+$id+')");
   
    if($query){
        echo 'success';
        $num_rows = mysql_num_rows($query);
        if($num_rows == 0){
            die("User Id Not Exist");
        }else if($num_rows > 1)
        {
            die("Not a unique id");
        }else{
            echo 'get id';
            $netPacket = array();
            $netPacket['DataType'] = "Member";
            
        }
    }else{
        die("server access failed");
    }
    
    $penguin_connect->close();
?>
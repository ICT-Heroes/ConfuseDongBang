<?php
    header('Content-Type: application/json; charset=UTF-8');
    include('connectDB.php');
    $netPacket = json_decode(file_get_contents('php://input'), true);
    
    $netPacket['classType'] = "Member";
    $netPacket['echoType'] = "NotEcho";
    $netPacket['func'] = $receivedPacket['func'];

    $jsonString = $netPacket['jsonString'];
    $member = json_decode($jsonString, true);
    
    $id = $member['id'];
    $password = $member['password'];
    $nickname = $member['nickname'];
    $emailAddress = $member['emailAddress'];
    $isAdmin = 0;
    $regData = "2016-12-20";
    $lastLogin = "2016-12-20";

    $query = $penguin_connect->query("INSERT INTO  `velmont`.`penguin_member_info` (
                                        `memberSrl` ,
                                        `id` ,
                                        `password` ,
                                        `nickname` ,
                                        `emailAddress` ,
                                        `isAdmin` ,
                                        `regDate` ,
                                        `lastLogin`
                                        ) VALUES (NULL, '$id','$password', '$nickname',  '$emailAddress', '$isAdmin', '$regDate', '$lastLogin')");
    if($query){
        $memberSrlQuery = $penguin_connect->query("SELECT `memberSrl` from `velmont`.`penguin_member_info` where `id` = '$id'");
        if(mysqli_num_rows($memberSrlQuery)>0){
            while($row = mysqli_fetch_assoc($memberSrlQuery)){
                $memberSrl = $row['memberSrl'];
            }
        }else{
            // 회원가입 실패
            $member['id'] = "";
            $netPacket['jsonString'] = json_encode($member);
            echo json_encode($netPacket);
        }
        
        $query2 = $penguin_connect->query("INSERT INTO  `velmont`.`penguin_player_state` (`memberSrl` ,`clientId` ,`characterKind` ,`hp` ,`maxHp` ,`posX` ,`posY` ,`posZ` ,
`quatX` ,`quatY` ,`quatZ` ,`quatW`) VALUES ('$memberSrl',  '3',  '1',  '1000',  '1000',  '0',  '0',  '0',  '0',  '0',  '0',  '1')");
        if($query2){
            $member['id'] = $id;
            $netPacket['jsonString'] = json_encode($member);
            echo json_encode($netPacket);
        }
    }else{
        // 회원가입 실패
        $member['id'] = "";
        $netPacket['jsonString'] = json_encode($member);
        echo json_encode($netPacket);
    }
    $penguin_connect->close();
?>
<?php

class MemberInfo{
    public $memberSrl;
    public $id;
    public $password;
    public $nickname;
    public $emailAddress;
    public $isAdmin;
    public $regDate;
    public $lastLogin;
}

class PlayerState{
    public $memberSrl;
    public $clientId;
    public $characterKind;
    public $pos;
    public $rot;
    public $hp;
    public $maxHp;
    public $charKind; 
}

class PlayerAttack {
    
}

class Quat{
    public $x;
    public $y;
    public $z;
    public $w;

    function __construct(){
        $a = func_get_args();
        $i = func_num_args();
        if(method_exists($this,$f='__construct'.$i)){
            call_user_func_array(array($this, $f), $a);
        }else{
            $this->x = 0;
            $this->y = 0;
            $this->z = 0;
            $this->w = 1;
        }
    }

    function __construct4($x, $y, $z, $w){
        $this->x = $x;
        $this->y = $y;
        $this->z = $z;
        $this->w = $w;
    }
}

class Vec3{
    public $x;
    public $y;
    public $z;

    function __construct(){
        $a = func_get_args();
        $i = func_num_args();
        if(method_exists($this,$f='__construct'.$i)){
            call_user_func_array(array($this, $f), $a);
        }else{
            $this->x = 0;
            $this->y = 0;
            $this->z = 0;
        }
    }

    function __construct3($x, $y, $z){
        $this->x = $x;
        $this->y = $y;
        $this->z = $z;
    }
}
?>
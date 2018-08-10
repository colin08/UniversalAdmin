// import axios from 'axios'
axios.interceptors.request.use((config)=>{
    //储存登录状态
    /*
    this.load=true;
    if(sessionStorage.USER_TOKEN){
        config.headers['Authorization']=sessionStorage.USER_TOKEN;
        config.headers['Content-Type']='application/json';
    }
    */
    //request请求前拦截，修改请求头部为一步请求
    // config.headers['X-Requested-With']='XMLHttpRequest';
    config.headers['Content-Type']='application/json';
    for(let item in config.data){
        if(config.data[item]==""){
            config.data[item]=null;
        }
    }
    return config;
},(error)=>{
    //请求错误处理
    this.load=false;
    console.error(error);
});

axios.interceptors.response.use((response)=>{
    //response请求返回时拦截，做统一的错误处理
    const data = response.data;
    /*
    this.load=false;

    if(!sessionStorage.USER_TOKEN){
        this.$router.push("/login");
    }
    */
    // 根据返回的code值来做不同的处理（和后端约定）
        switch (data.status) {
            /*
            case 200:
                return data;
                */
        
            // 需要重新登录
            case 300:
                localStorage.setItem("TURN_URL",window.location.href);
                window.location.href='/login';                
                break;
        
            default:
                return data;
                console.log(data.message);
        }
        // 若不是正确的返回code，且已经登录，就抛出错误
        const error = new Error(data.description);
    
        error.data = data;
        error.response = response;
    
        throw error;
},(error)=>{
    this.load=false;
    if(error&&error.response){
        //请求返回错误，并有错误内容
        switch(error.response.status){
            case 400:
            error.message='请求错误';
            break;

            case 401:
            error.message='未授权，请登陆';
            break;

            case 403:
            error.message='服务器拒绝访问';
            break;

            case 404:
            error.message=`请求地址错误：${error.response.config.url}`;
            break;

            case 405:
            error.message='请求方法不正确';
            break;

            case 408:
            error.message='请求超时';
            break;

            case 500:
            error.message='服务器内部错误';
            break;

            case 501:
            error.message='服务未实现';
            break;

            case 502:
            error.message='网关错误';
            break;

            case 503:
            error.message='服务不可用';
            break;

            case 504:
            error.message='网关超时';
            break;

            case 505:
            error.message='HTTP版本不受支持';
            break;

            default:
        }
    }
    return Promise.reject(error);
});
export default axios;
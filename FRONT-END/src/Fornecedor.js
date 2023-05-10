import React,{Component} from "react";
import { variables } from "./Variables.js";

export class Fornecedor extends Component{
    
    constructor(props){
        super(props);

        this.state={
            empresas:[],
            fornecedores:[],
            modalTitle:"",
            id:0,
            nome:"",
            cnpjCpf:"",
            rg:"",
            cep:"",
            dataNascimento:"",
            email:"",
            tipoFornecedor: 0,
            fornecedorEmpresa:""
        }
    }

    refreshList(){
        fetch(variables.API_URL+'fornecedores')
        .then(response=>response.json())
        .then(data=>{
            console.log(data);
            this.setState({fornecedores:data});
        })

        fetch(variables.API_URL+'empresas')
        .then(response=>response.json())
        .then(data=>{
            console.log(data);
            this.setState({empresas:data});
        })
    }

    componentDidMount(){
        this.refreshList();
    }

    changeNome=(f)=>{
        this.setState({nome:f.target.value});
    }

    changeCnpjCpf=(f)=>{
        this.setState({cnpjCpf:f.target.value});
    }

    changeCep=(f)=>{
        this.setState({cep:f.target.value});
    }

    changeRg=(f)=>{
        this.setState({rg:f.target.value});
    }

    changeDataNascimento=(f)=>{
        this.setState({dataNascimento:f.target.value});
    }

    changeEmail=(f)=>{
        this.setState({email:f.target.value});
    }

    addClick(){
        this.setState({
            modalTitle:"Adicionar Fornecedor",
            id:0,
            nome:"",
            cnpjCpf:"",
            rg:"",
            cep:"",
            dataNascimento:"",
            email:"",
            tipoFornecedor: 0,
            fornecedorEmpresa:""
        })
    }

    editClick(forn){
        this.setState({
            modalTitle:"Editar Fornecedor",
            id:forn.id,
            nome:forn.nome,
            cnpjCpf:forn.cnpjCpf,
            rg:forn.rg,
            cep:forn.cep,
            dataNascimento:forn.dataNascimento,
            email:forn.email,
            tipoFornecedor: forn.tipoFornecedor,
            fornecedorEmpresa:forn.fornecedorEmpresa
        })
    }

    createClick(){
        fetch(variables.API_URL+'fornecedores',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json'
            },
            body:JSON.stringify({
                nome:this.state.nome,
                cnpjCpf:this.state.cnpjCpf,
                rg:this.state.rg,
                cep:this.state.cep,
                dataNascimento:this.state.dataNascimento,
                email:this.state.email,
                tipoFornecedor: this.state.tipoFornecedor,
                fornecedorEmpresa:this.state.fornecedorEmpresa
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            this.refreshList();
        },(error)=>{
            alert('Failed');
        })
    }

    updateClick(id){
        fetch(variables.API_URL+'fornecedores/'+id,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'X-HTTP-Method-Override': 'PUT'
            },
            body:JSON.stringify({
                id:this.state.id,
                nome:this.state.nome,
                cnpjCpf:this.state.cnpjCpf,
                rg:this.state.rg,
                cep:this.state.cep,
                dataNascimento:this.state.dataNascimento,
                email:this.state.email,
                tipoFornecedor: (this.state.cnpjCpf.toString() > 11)?1:0,
                fornecedorEmpresa:this.state.fornecedorEmpresa
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            this.refreshList();
        },(error)=>{
            alert('Failed');
        })
    }

    deleteClick(id){
        if(window.confirm('Você tem certeza?')){
        fetch(variables.API_URL+'fornecedores/'+id,{
            method:'DELETE',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'X-HTTP-Method-Override': 'PUT'
            }
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
            this.refreshList();
        },(error)=>{
            alert('Failed');
        })
        }
    }


    render(){
        const{
            fornecedores,
            modalTitle,
            id,
            nome,
            cnpjCpf,
            rg,
            cep,
            dataNascimento,
            email,
            tipoFornecedor,
            fornecedorEmpresa
        }=this.state;


        return(
            <div>
                <button type="button"
                className="btn btn-primary m-2 float-end"
                data-bs-toggle="modal"
                data-bs-target="#exampleModal"
                onClick={()=>this.addClick()}>
                    Adicionar Fornecedor
                </button>

                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>
                                Id
                            </th>
                            <th>
                                CPF / CNPJ
                            </th>
                            <th>
                                Nome
                            </th>
                            <th>
                                RG
                            </th>
                            <th>
                                CEP
                            </th>
                            <th>
                                Data de Nascimento
                            </th>
                            <th>
                                E-mail
                            </th>
                            <th>
                                Empresas
                            </th>
                            <th>
                                Opções
                            </th>
                        </tr>   	
                    </thead>
                    <tbody>
                        {fornecedores.map(forn =>
                            <tr key={forn.id}>
                                <td>{forn.id}</td>
                                <td>{forn.cnpjCpf}</td>
                                <td>{forn.nome}</td>
                                <td>{forn.rg}</td>
                                <td>{forn.cep}</td>
                                <td>{forn.dataNascimento}</td>
                                <td>{forn.email}</td>
                                <td>{forn.fornecedorEmpresa.map(fornecedorEmpresa => (
                                    <tr key={fornecedorEmpresa.id}>
                                        <td>{fornecedorEmpresa.empresa.nomeFantasia}</td>
                                    </tr>
                                    ))}
                                
                                </td>
                                <td>
                                    <button type="button"
                                    className="btn btn-light mr-1"
                                    data-bs-toggle="modal"
                                    data-bs-target="#exampleModal"
                                    onClick={()=>this.editClick(forn)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                        <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
                                        </svg>
                                    </button>

                                    <button type="button"
                                    className="btn btn-light mr-1"
                                    onClick={()=>this.deleteClick(forn.id)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                        <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>
                                        </svg>
                                    </button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>

                <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered">
                <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">{modalTitle}</h5>
                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"
                    ></button>
                </div>

                    <div className="modal-body">
                        <div className="input-group mb-3">
                            <span className="input-group-text">CPF/CNPJ</span>
                            <input type="text" className="form-control"
                            value={cnpjCpf}
                            onChange={this.changeCnpjCpf}/>
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">Nome</span>
                            <input type="text" className="form-control"
                            value={nome}
                            onChange={this.changeNome}/>
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">CEP</span>
                            <input type="text" className="form-control"
                            value={cep}
                            onChange={this.changeCep}/>
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">RG</span>
                            <input type="text" className="form-control"
                            value={rg}
                            onChange={this.changeRg}/>
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">Data de Nascimento</span>
                            <input type="date" className="form-control"
                            value={dataNascimento}
                            onChange={this.changeDataNascimento}/>
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">E-mail</span>
                            <input type="text" className="form-control"
                            value={email}
                            onChange={this.changeEmail}/>
                        </div>

                        {id===0?
                        <button type="button"
                        className="btn btn-primary float-start"
                        onClick={()=>this.createClick()}
                        >Criar</button>
                        :null}

                        {id!==0?
                        <button type="button"
                        className="btn btn-primary float-start"
                        onClick={()=>this.updateClick(id)}
                        >Atualizar</button>
                        :null}

                    </div>
                </div>
                </div> 
                </div>
            </div>
        )
    }
}
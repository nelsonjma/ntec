
using System.Collections.Generic;

using DbConfig;

/// <summary>
/// Check if user has authorization to access the page
/// </summary>
public class PageAuthentication
{
    /* 
        valida se a pagina está associada a users 
        validar se o user esta autenticado
            se existir
                 validar se pagina tem autenticação e caso tenho então verifica se o user tem acesso 
            se não existir
                se a pagina tiver autenticação e o user não existir õu naõ tiver acesso então aponta para o frontoffice...
     * 
     *        
    */
    public PageAuthentication()
	{

	}

    private List<Page> GetUserPages ()
    {
     
    }


}
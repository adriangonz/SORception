/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package clientepruebasjava;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import org.datacontract.schemas._2004._07.managersystem.Taller;

/**
 *
 * @author Ruben
 */
public class ClientePruebasJava {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        try {
            Taller tall = addNewTaller("Taller 1");
            System.out.println(tall.getId());
            System.out.println(tall.getNombre().getValue());
            
            System.out.println(addNewDesguace("asdf"));
        } catch (Exception e1) {
            System.out.println(e1.getMessage());
        }
    }

    private static Taller addNewTaller(String name) {
        org.tempuri.GestionTaller service = new org.tempuri.GestionTaller();
        org.tempuri.IGestionTaller port = service.getBasicHttpBindingIGestionTaller();
        return port.addNewTaller(name);
    }
    
    private static Integer addNewDesguace(String name) {
        org.tempuri.GestionDesguace service = new org.tempuri.GestionDesguace();
        org.tempuri.IGestionDesguace port = service.getBasicHttpBindingIGestionDesguace();
        return port.addNewDesguace(name);
    }

}

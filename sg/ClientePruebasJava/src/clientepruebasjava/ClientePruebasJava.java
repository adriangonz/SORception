/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package clientepruebasjava;

import java.io.BufferedReader;
import java.io.InputStreamReader;

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
            Integer eid;
            BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

            System.out.println("Enter number");
            eid= Integer.parseInt(br.readLine());

            System.out.println(addNewTaller("Taller 1"));
        } catch (Exception e1) {
            System.out.println(e1.getMessage());
        }
    }

    private static Integer addNewTaller(String name) {
        org.tempuri.GestionTaller service = new org.tempuri.GestionTaller();
        org.tempuri.IGestionTaller port = service.getBasicHttpBindingIGestionTaller();
        return port.addNewTaller(name);
    }

}
